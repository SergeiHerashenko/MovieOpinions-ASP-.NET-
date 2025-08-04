using MovieOpinions.server.Domain.Model.Actors;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.DAL.Interface
{
    public interface IActorRepository : IBaseRepository<Actor>
    {
        Task<BaseResponse<IEnumerable<Actor>>> GetActorByFilmId(int idFilm);
    }
}
