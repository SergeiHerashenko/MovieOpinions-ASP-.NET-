using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Domain.Response;
using MovieOpinions.server.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginModel LoginModel)
        {
            try
            {
                var GetUser = await _userRepository.GetUser(LoginModel.LoginUser);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }

            return new BaseResponse<ClaimsIdentity>()
            {
                Description = "1123",
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }
}
