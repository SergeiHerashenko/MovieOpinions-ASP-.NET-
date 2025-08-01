using MovieOpinions.server.Domain.Model;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Interfaces
{
    public interface IGenreService
    {
        Task<BaseResponse<IEnumerable<Genre>>> GetAllGenre();
    }
}
