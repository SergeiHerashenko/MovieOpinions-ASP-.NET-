using MovieOpinions.server.DAL.Connect_Database;
using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model.Movie;
using MovieOpinions.server.Domain.Response;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.DAL.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly IConnectMovieOpinions _connectMovieOpinions;

        public FilmRepository(IConnectMovieOpinions connectMovieOpinions)
        {
            _connectMovieOpinions = connectMovieOpinions;
        }   

        public Task<BaseResponse<Film>> Create(Film entity)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> Delete(Film entity)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IEnumerable<Film>>> GetAllFilms()
        {
            var films = new List<Film>();

            using (var conn = new NpgsqlConnection(_connectMovieOpinions.GetConnectMovieOpinionsDataBase()))
            {
                try
                {
                    await conn.OpenAsync();

                    await using (var getAllFilms = new NpgsqlCommand(
                        "SELECT " +
                            "id_film, name_film, year_film " +
                        "FROM " +
                            "Film_Table", conn))
                    {
                        using (var reader = await getAllFilms.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Film film = new Film()
                                {
                                    IdFilm = Convert.ToInt32(reader["id_film"]),
                                    NameFilm = Convert.ToString(reader["name_film"]),
                                    YearFilm = Convert.ToInt32(reader["year_film"])
                                };

                                films.Add(film);
                            }

                            if (films.Count == 0)
                            {
                                return new BaseResponse<IEnumerable<Film>>()
                                {
                                    Description = "Фільмів не знайдено",
                                    StatusCode = Domain.Enum.StatusCode.NotFound
                                };
                            }

                            return new BaseResponse<IEnumerable<Film>>()
                            {
                                Data = films,
                                StatusCode = Domain.Enum.StatusCode.OK
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponse<IEnumerable<Film>>()
                    {
                        StatusCode = Domain.Enum.StatusCode.InternalServerError,
                        Description = ex.Message
                    };
                }
            }
        }

        public Task<BaseResponse<Film>> Update(Film entity)
        {
            throw new NotImplementedException();
        }
    }
}
