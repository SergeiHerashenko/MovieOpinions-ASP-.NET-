using MovieOpinions.server.Domain.Model.Movie;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.DAL.Interface
{
    public interface IFilmRepository : IBaseRepository<Film>
    {
        Task<BaseResponse<IEnumerable<Film>>> GetAllFilms();
        Task<BaseResponse<Film>> GetFilmById(int idFilm);
        Task<BaseResponse<DetailedFilm>> GetFilmDetailed(int idFilm);
    }
}
