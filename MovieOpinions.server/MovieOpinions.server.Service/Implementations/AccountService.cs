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

        public async Task<BaseResponse<User>> Login(LoginModel LoginModel)
        {
            try
            {
                var GetUser = await _userRepository.GetUser(LoginModel.LoginUser);

                if(GetUser.StatusCode != Domain.Enum.StatusCode.OK)
                {
                    return new BaseResponse<User>
                    {
                        Description = GetUser.Description,
                        StatusCode = GetUser.StatusCode,
                    };
                }

                bool IsPasswordCorrect = await new CheckingCorrectnessPassword().VerifyPassword(
                    LoginModel.PasswordUser,
                    GetUser.Data.Security.PasswordSalt,
                    GetUser.Data.Security.PasswordHash);

                if (!IsPasswordCorrect)
                {
                    return new BaseResponse<User>
                    {
                        Description = "Невірний логін або пароль!",
                        StatusCode = Domain.Enum.StatusCode.NotFound
                    };
                }

                var IsAccess =  CheckUserAccess(GetUser);

                return IsAccess;
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

        public async Task<BaseResponse<User>> Registartion(RegistrationModel RegistrationModel)
        {
            try
            {
                var GetUser = await _userRepository.GetUser(RegistrationModel.LoginUser);

                if(GetUser.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return new BaseResponse<User>
                    {
                        Description = "Користувач з таким логіном вже зареєстрований!",
                        StatusCode = Domain.Enum.StatusCode.Conflict
                    };
                }

                if(GetUser.StatusCode == Domain.Enum.StatusCode.NotFound)
                {
                    string PasswordSalt = Guid.NewGuid().ToString();
                    string EncryptionPassword = await new HashPassword().GetHashedPassword(RegistrationModel.PasswordUser, PasswordSalt);

                    var NewUser = new User()
                    {
                        UserId = Guid.NewGuid(),
                        LoginUser = RegistrationModel.LoginUser,
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
                            PasswordHash = EncryptionPassword,
                            PasswordSalt = PasswordSalt,
                            FailedLoginAttempts = 0,
                            IsBlocked = false,
                            IsDeleted = false,
                            IsEmailConfirmed = false,
                            LastLoginDate = DateTime.Now,
                        }
                    };

                    var RegisterUser = await _userRepository.Create(NewUser);

                    if (RegisterUser.Data != null)
                    {
                        return new BaseResponse<User>()
                        {
                            Data = RegisterUser.Data,
                            Description = "Користувач зареєстрований!",
                            StatusCode = Domain.Enum.StatusCode.OK
                        };
                    }
                    else
                    {
                        return new BaseResponse<User>()
                        {
                            Description = RegisterUser.Description,
                            StatusCode = RegisterUser.StatusCode
                        };
                    }
                }
                else
                {
                    return new BaseResponse<User>()
                    {
                        Description = GetUser.Description,
                        StatusCode = GetUser.StatusCode
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
            //var key = _configuration["Jwt:Key"];
            //var issuer = _configuration["Jwt:Issuer"];
            //var audience = _configuration["Jwt:Audience"];
            //
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Claims = new Dictionary<string, object>
            //    {
            //        {"uid", "test-user-2" },
            //        {"fullname", "Park Tokki" },
            //        {"email", "tokki.park@test.com" },
            //        {"ts", 1596611792131L },
            //    },
            //    SigningCredentials = new SigningCredentials(
            //        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this_is_test_secret_key_at_least_32_char")),
            //        SecurityAlgorithms.HmacSha256Signature
            //    )
            //};
            //
            //var stoken = tokenHandler.CreateToken(tokenDescriptor);
            //var token = tokenHandler.WriteToken(stoken);

            // Крок 1: Claims
            var nameIdentifier = user.UserId.ToString();
            var login = user.LoginUser;

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, nameIdentifier),
        new Claim(ClaimTypes.Name, login)
    };

            // Крок 2: Ключ
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new Exception("JWT Key is missing or empty!");

            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
            if (keyBytes.Length < 32)
                throw new Exception("JWT Key must be at least 32 bytes!");

            var securityKey = new SymmetricSecurityKey(keyBytes);

            // Крок 3: Підпис
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Крок 4: Інші параметри
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expires = DateTime.UtcNow.AddHours(1);

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                throw new Exception("Issuer or Audience is missing");

            // Крок 5: Створення токена
            var jwtToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            // Крок 6: Генерація рядка
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return tokenString;

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            //    new Claim(ClaimTypes.Name, user.LoginUser),
            //    new Claim(ClaimTypes.Role, user.Role.ToString())
            //};
            //
            //var key = new SymmetricSecurityKey(
            //    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //
            //var token = new JwtSecurityToken(
            //    issuer: _configuration["Jwt:Issuer"],
            //    audience: _configuration["Jwt:Audience"],
            //    claims: claims,
            //    expires: DateTime.UtcNow.AddHours(1),
            //    signingCredentials: creds
            //);
            //
            //return new JwtSecurityTokenHandler().WriteToken(token);;
            //return token;
        }

        private BaseResponse<User> CheckUserAccess(BaseResponse<User> user)
        {
            if (user.Data.Security.IsBlocked)
            {
                return new BaseResponse<User>()
                {
                    Description = "Користувач заблокований!",
                    StatusCode = Domain.Enum.StatusCode.Forbidden
                };
            }

            if(user.Data.Security.IsDeleted)
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
                Data = user.Data
            };
        }
    }
}
