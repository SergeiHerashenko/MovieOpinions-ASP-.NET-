using MovieOpinions.server.Domain.Model.Comments;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Interfaces
{
    public interface ICommentService
    {
        Task<BaseResponse<List<Comment>>> GetAllCommentFilm(int idFilm);
    }
}
