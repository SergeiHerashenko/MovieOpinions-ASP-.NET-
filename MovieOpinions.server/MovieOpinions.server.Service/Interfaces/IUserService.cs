using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<UserEntity>> GetUserByIdAsync(Guid idUser);

        Task<BaseResponse<UserProfile>> GetUserProfileById(Guid idUser);
    }
}
