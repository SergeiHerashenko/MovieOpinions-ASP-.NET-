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
        private readonly IGenreService _genreService;
        private readonly IActorService _actorService;
        private readonly ICountryService _countryService;

        public FilmService(IFilmRepository filmRepository, IGenreService genreService, IActorService actorService, ICountryService countryService)
        {
            _filmRepository = filmRepository;
            _genreService = genreService;
            _actorService = actorService;
            _countryService = countryService;
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
                allFilms.ImageFilm = GenerateFilmImageUrl(allFilms.NameFilm);
            }

            return new BaseResponse<IEnumerable<Film>>()
            {
                Data = getAllFilms.Data,
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }

        public async Task<BaseResponse<DetailedFilm>> GetFilm(int idFilm)
        {
            var getFilmDetailed = await _filmRepository.GetFilmDetailed(idFilm);

            if (getFilmDetailed.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return new BaseResponse<DetailedFilm>()
                {
                    Description = getFilmDetailed.Description,
                    StatusCode = getFilmDetailed.StatusCode,
                };
            }

            var detailedFilm = getFilmDetailed.Data;

            var getFilm = await _filmRepository.GetFilmById(idFilm);
            
            if(getFilm.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return new BaseResponse<DetailedFilm>()
                {
                    Description = getFilm.Description,
                    StatusCode = getFilm.StatusCode,
                };
            }

            detailedFilm.NameFilm = getFilm.Data.NameFilm;
            detailedFilm.YearFilm = getFilm.Data.YearFilm;
            detailedFilm.ImageFilm = GenerateFilmImageUrl(detailedFilm.NameFilm);

            var getGenreFilm = await _genreService.GetGenreFilm(idFilm);
            
            if(getGenreFilm.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return new BaseResponse<DetailedFilm>()
                {
                    Description = getGenreFilm.Description,
                    StatusCode = getGenreFilm.StatusCode,
                };
            }

            detailedFilm.GenreFilm = getGenreFilm.Data;

            var getActorFilm = await _actorService.GetActorFilm(idFilm);

            if(getActorFilm.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return new BaseResponse<DetailedFilm>()
                {
                    Description = getActorFilm.Description,
                    StatusCode = getActorFilm.StatusCode,
                };
            }

            detailedFilm.ActorFilm = getActorFilm.Data;

            var getCountryFilm = await _countryService.GetCountryByFilm(idFilm);

            if(getCountryFilm.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return new BaseResponse<DetailedFilm>()
                {
                    Description = getCountryFilm.Description,
                    StatusCode = getCountryFilm.StatusCode,
                };
            }

            detailedFilm.CountryFilm = getCountryFilm.Data;

            return new BaseResponse<DetailedFilm>()
            {
                Data = detailedFilm,
                StatusCode = Domain.Enum.StatusCode.OK
            };
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

        private string GenerateFilmImageUrl(string filmName)
        {
            string[] words = Regex.Split(filmName, @"\W+");
            return $"https://localhost:7230/Image/Film/{string.Join("_", words)}.jpg";
        }
    }
}
