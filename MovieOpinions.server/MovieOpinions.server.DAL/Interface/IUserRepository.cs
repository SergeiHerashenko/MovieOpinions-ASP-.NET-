using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.DAL.Interface
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<BaseResponse<UserEntity>> GetUser(string loginUser);

        Task<BaseResponse<UserEntity>> GetUserId(Guid idUser);

        Task<BaseResponse<bool>> BlockUser(UserEntity user);
    }
}
