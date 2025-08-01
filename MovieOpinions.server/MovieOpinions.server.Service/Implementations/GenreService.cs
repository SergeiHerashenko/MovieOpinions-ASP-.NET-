using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model;
using MovieOpinions.server.Domain.Response;
using MovieOpinions.server.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository  genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<BaseResponse<IEnumerable<Genre>>> GetAllGenre()
        {
            var getAllGenre = await _genreRepository.GetAllGenre();

            if (getAllGenre.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return new BaseResponse<IEnumerable<Genre>>()
                {
                    Description = getAllGenre.Description,
                    StatusCode = getAllGenre.StatusCode
                };
            }

            return new BaseResponse<IEnumerable<Genre>>()
            {
                Data = getAllGenre.Data,
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
    }
}
