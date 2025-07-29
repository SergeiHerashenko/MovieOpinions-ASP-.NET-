using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model.Movie;
using MovieOpinions.server.Domain.Response;
using MovieOpinions.server.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var response = await _filmRepository.GetAllFilms();

            return response;
        }

        public async Task<BaseResponse<DetailedFilm>> GetFilm(int idFilm)
        {
            throw new NotImplementedException();
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
