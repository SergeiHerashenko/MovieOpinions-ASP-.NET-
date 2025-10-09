using MovieOpinions.server.Domain.Model.Comments;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.DAL.Interface
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<BaseResponse<IEnumerable<Comment>>> GetCommentFilm(int idFilm);
    }
}
