using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model.Actors;
using MovieOpinions.server.Domain.Response;
using MovieOpinions.server.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Implementations
{
    public class ActorService : IActorService
    {
        readonly IActorRepository _actorRepository;

        public ActorService(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public async Task<BaseResponse<IEnumerable<Actor>>> GetActorFilm(int idFilm)
        {
            var response =  await _actorRepository.GetActorByFilmId(idFilm);

            if(response.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return new BaseResponse<IEnumerable<Actor>>()
                {
                    Description = response.Description,
                    StatusCode = response.StatusCode,
                };
            }

            return new BaseResponse<IEnumerable<Actor>>()
            {
                Data = response.Data,
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
    }
}
