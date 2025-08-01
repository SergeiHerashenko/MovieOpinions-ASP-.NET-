using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model.Movie;
using MovieOpinions.server.Domain.Response;
using MovieOpinions.server.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Implementations
{
    public class FilmService : IFilmService
    {
        private readonly IFilmRepository _filmRepository;

        public FilmService(IFilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }

        public async Task<BaseResponse<IEnumerable<Film>>> GetAllFilms()
        {
            var getAllFilms = await _filmRepository.GetAllFilms();

            if(getAllFilms.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return new BaseResponse<IEnumerable<Film>>()
                {
                    Description = getAllFilms.Description,
                    StatusCode = getAllFilms.StatusCode,
                };
            }

            foreach(var allFilms in getAllFilms.Data)
            {
                string[] words = Regex.Split(allFilms.NameFilm, @"\W+");
                string filmImage = $"https://localhost:7230/Image/Film/{string.Join("_", words)}.jpg";
                allFilms.ImageFilm = filmImage;
            }

            return new BaseResponse<IEnumerable<Film>>()
            {
                Data = getAllFilms.Data,
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }

        public async Task<BaseResponse<DetailedFilm>> GetFilm(int idFilm)
        {
            var getFilm = await _filmRepository.GetFilmById(idFilm);

            if(getFilm.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return new BaseResponse<DetailedFilm>()
                {
                    Description = getFilm.Description,
                    StatusCode = getFilm.StatusCode,
                };
            }


        }

        public async Task<BaseResponse<IEnumerable<Film>>> GetFilmByGenre(int idGenre)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IEnumerable<Film>>> GetFilmByYear(IEnumerable<string> selectedYear)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IEnumerable<Film>>> SortingFilm(string sortElement)
        {
            throw new NotImplementedException();
        }
    }
}
