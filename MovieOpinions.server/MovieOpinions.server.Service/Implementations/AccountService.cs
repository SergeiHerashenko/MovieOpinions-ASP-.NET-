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

        public Task<BaseResponse<User>> Registartion(RegistrationModel RegistrationModel)
        {
            throw new NotImplementedException();
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Profile.FirstName),
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
