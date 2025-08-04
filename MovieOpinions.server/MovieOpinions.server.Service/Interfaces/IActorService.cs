using MovieOpinions.server.Domain.Model.Actors;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Interfaces
{
    public interface IActorService
    {
        Task<BaseResponse<IEnumerable<Actor>>> GetActorFilm(int idFilm);
    }
}
