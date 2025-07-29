using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.DAL.Interface
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<BaseResponse<User>> GetUser(string loginUser);
        Task<BaseResponse<User>> GetUserId(int id);
        Task<BaseResponse<bool>> BlockUser(User user);
    }
}
