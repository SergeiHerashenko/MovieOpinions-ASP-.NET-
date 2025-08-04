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

                            if (!films.Any())
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

        public async Task<BaseResponse<Film>> GetFilmById(int idFilm)
        {
            using(var conn =  new NpgsqlConnection(_connectMovieOpinions.GetConnectMovieOpinionsDataBase()))
            {
                try
                {
                    await conn.OpenAsync();

                    await using(var getFilm = new NpgsqlCommand(
                        "SELECT " +
                            "id_film, name_film, year_film " +
                        "FROM " +
                            "Film_Table " +
                        "WHERE " +
                            "id_film = @ID_FILM", conn))
                    {
                        getFilm.Parameters.AddWithValue("ID_Film", idFilm);

                        using (var reader = await getFilm.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                Film film = new Film()
                                {
                                    IdFilm = Convert.ToInt32(reader["id_film"]),
                                    NameFilm = reader["name_film"].ToString(),
                                    YearFilm = Convert.ToInt32(reader["year_film"])
                                };

                                return new BaseResponse<Film>()
                                {
                                    Data = film,
                                    StatusCode = Domain.Enum.StatusCode.OK
                                };
                            }

                            return new BaseResponse<Film>()
                            {
                                StatusCode = Domain.Enum.StatusCode.NotFound,
                                Description = "Фільм не знайдено"
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponse<Film>()
                    {
                        StatusCode = Domain.Enum.StatusCode.InternalServerError,
                        Description = ex.Message
                    };
                }
            }
        }

        public async Task<BaseResponse<DetailedFilm>> GetFilmDetailed(int idFilm)
        {
            using (var conn = new NpgsqlConnection(_connectMovieOpinions.GetConnectMovieOpinionsDataBase()))
            {
                try
                {
                    await conn.OpenAsync();

                    await using (var getDetailsFilm = new NpgsqlCommand(
                        "SELECT " +
                            "id_film, director_film, description_film " +
                        "FROM " +
                            "Film_Details_Table " +
                        "WHERE " +
                            "id_film = @ID_FILM", conn))
                    {
                        getDetailsFilm.Parameters.AddWithValue("ID_Film", idFilm);

                        using (var reader = await getDetailsFilm.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                DetailedFilm film = new DetailedFilm()
                                {
                                    IdFilm = Convert.ToInt32(reader["id_film"]),
                                    DirectorFilm = reader["director_film"].ToString(),
                                    DescriptionFilm = reader["description_film"].ToString()
                                };

                                return new BaseResponse<DetailedFilm>()
                                {
                                    Data = film,
                                    StatusCode = Domain.Enum.StatusCode.OK
                                };
                            }

                            return new BaseResponse<DetailedFilm>()
                            {
                                StatusCode = Domain.Enum.StatusCode.NotFound,
                                Description = "Фільм не знайдено"
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponse<DetailedFilm>()
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
