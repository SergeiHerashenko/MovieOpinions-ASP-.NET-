using MovieOpinions.server.DAL.Connect_Database;
using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model;
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
    public class GenreRepository : IGenreRepository
    {
        private readonly IConnectMovieOpinions _connectMovieOpinions;

        public GenreRepository(IConnectMovieOpinions connectMovieOpinions)
        {
            _connectMovieOpinions = connectMovieOpinions;
        }

        public Task<BaseResponse<Genre>> Create(Genre entity)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> Delete(Genre entity)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IEnumerable<Genre>>> GetAllGenre()
        {
            var genres = new List<Genre>();

            using (var conn = new NpgsqlConnection(_connectMovieOpinions.GetConnectMovieOpinionsDataBase()))
            {
                try
                {
                    await conn.OpenAsync();

                    await using (var getAllGenre = new NpgsqlCommand(
                        "SELECT " +
                            "id_genre, name_genre " +
                        "FROM " +
                            "Genre_Table;", conn))
                    {
                        using (var reader = await getAllGenre.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Genre genre = new Genre()
                                {
                                    IdGenre = Convert.ToInt32(reader["id_genre"]),
                                    NameGenre = reader["name_genre"].ToString()
                                };

                                genres.Add(genre);
                            }

                            if(genres.Count == 0)
                            {
                                return new BaseResponse<IEnumerable<Genre>>()
                                {
                                    Description = "Жанрів не знайдено",
                                    StatusCode = Domain.Enum.StatusCode.NotFound
                                };
                            }

                            return new BaseResponse<IEnumerable<Genre>>()
                            {
                                Data = genres,
                                StatusCode = Domain.Enum.StatusCode.OK
                            };
                        }
                    } 
                }
                catch (Exception ex)
                {
                    return new BaseResponse<IEnumerable<Genre>>()
                    {
                        StatusCode = Domain.Enum.StatusCode.InternalServerError,
                        Description = ex.Message
                    };
                }
            } 
        }

        public Task<BaseResponse<Genre>> Update(Genre entity)
        {
            throw new NotImplementedException();
        }
    }
}
