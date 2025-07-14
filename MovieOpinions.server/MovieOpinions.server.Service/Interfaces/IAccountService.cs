using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<User>> Login(LoginModel LoginModel);

        Task<BaseResponse<User>> Registartion(RegistrationModel RegistrationModel);

        string GenerateJwtToken(User user);
    }
}
