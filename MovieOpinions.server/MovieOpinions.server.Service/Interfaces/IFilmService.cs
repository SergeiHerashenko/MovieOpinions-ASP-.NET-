using MovieOpinions.server.Domain.Model.Movie;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Interfaces
{
    public interface IFilmService
    {
        Task<BaseResponse<DetailedFilm>> GetFilm(int idFilm);
        Task<BaseResponse<IEnumerable<Film>>> GetFilmByGenre(int idGenre);
        Task<BaseResponse<IEnumerable<Film>>> GetFilmByYear(IEnumerable<string> selectedYear);
        Task<BaseResponse<IEnumerable<Film>>> SortingFilm(string sortElement);
        Task<BaseResponse<IEnumerable<Film>>> GetAllFilms();
    }
}
