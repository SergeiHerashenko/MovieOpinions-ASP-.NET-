using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Enum;
using MovieOpinions.server.Domain.Helpers;
using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Domain.Response;
using MovieOpinions.server.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AccountService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<BaseResponse<User>> Login(LoginModel loginModel)
        {
            try
            {
                var getUser = await _userRepository.GetUser(loginModel.LoginUser);

                if(getUser.StatusCode != Domain.Enum.StatusCode.OK)
                {
                    return new BaseResponse<User>
                    {
                        Description = getUser.Description,
                        StatusCode = getUser.StatusCode,
                    };
                }

                bool isPasswordCorrect = await new CheckingCorrectnessPassword().VerifyPassword(
                    loginModel.PasswordUser,
                    getUser.Data.Security.PasswordSalt,
                    getUser.Data.Security.PasswordHash);

                if (!isPasswordCorrect)
                {
                    return new BaseResponse<User>
                    {
                        Description = "Невірний логін або пароль!",
                        StatusCode = Domain.Enum.StatusCode.Forbidden
                    };
                }

                var isAccess =  CheckUserAccess(getUser);

                return isAccess;
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<User>> Registration(RegistrationModel registrationModel)
        {
            try
            {
                var getUser = await _userRepository.GetUser(registrationModel.LoginUser);

                if(getUser.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return new BaseResponse<User>
                    {
                        Description = "Користувач з таким логіном вже зареєстрований!",
                        StatusCode = Domain.Enum.StatusCode.Conflict
                    };
                }

                if(getUser.StatusCode == Domain.Enum.StatusCode.NotFound)
                {
                    string passwordSalt = Guid.NewGuid().ToString();
                    string encryptionPassword = await new HashPassword().GetHashedPassword(registrationModel.PasswordUser, passwordSalt);

                    var newUser = new User()
                    {
                        UserId = Guid.NewGuid(),
                        LoginUser = registrationModel.LoginUser,
                        EmailUser = null,
                        Role = 0,

                        Profile = new UserProfile()
                        {
                            FirstName = null,
                            LastName = null,
                            Bio = null,
                            AvatarUrl = null,
                            UpdatedAt = null,
                            CreatedAt = DateTime.Now,
                        },

                        Security = new UserSecurity()
                        {
                            PasswordHash = encryptionPassword,
                            PasswordSalt = passwordSalt,
                            FailedLoginAttempts = 0,
                            IsBlocked = false,
                            IsDeleted = false,
                            IsEmailConfirmed = false,
                            LastLoginDate = DateTime.Now,
                        }
                    };

                    var registerUser = await _userRepository.Create(newUser);

                    if (registerUser.Data != null)
                    {
                        return new BaseResponse<User>()
                        {
                            Data = registerUser.Data,
                            Description = "Користувач зареєстрований!",
                            StatusCode = Domain.Enum.StatusCode.OK
                        };
                    }
                    else
                    {
                        return new BaseResponse<User>()
                        {
                            Description = registerUser.Description,
                            StatusCode = registerUser.StatusCode
                        };
                    }
                }
                else
                {
                    return new BaseResponse<User>()
                    {
                        Description = getUser.Description,
                        StatusCode = getUser.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.LoginUser),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private BaseResponse<User> CheckUserAccess(BaseResponse<User> userResponse)
        {
            if (userResponse.Data.Security.IsBlocked)
            {
                return new BaseResponse<User>()
                {
                    Description = "Користувач заблокований!",
                    StatusCode = Domain.Enum.StatusCode.Forbidden
                };
            }

            if(userResponse.Data.Security.IsDeleted)
            {
                return new BaseResponse<User>
                {
                    Description = "Користувач видалений!",
                    StatusCode = Domain.Enum.StatusCode.Gone
                };
            }

            return new BaseResponse<User>
            {
                Description = "Користувач має доступ!",
                StatusCode = Domain.Enum.StatusCode.OK,
                Data = userResponse.Data
            };
        }
    }
}
