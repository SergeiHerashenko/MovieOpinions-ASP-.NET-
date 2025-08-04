using MovieOpinions.server.DAL.Connect_Database;
using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model.Actors;
using MovieOpinions.server.Domain.Response;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.DAL.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly IConnectMovieOpinions _connectMovieOpinions;

        public ActorRepository(IConnectMovieOpinions connectMovieOpinions)
        {
            _connectMovieOpinions = connectMovieOpinions;
        }

        public Task<BaseResponse<Actor>> Create(Actor entity)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> Delete(Actor entity)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IEnumerable<Actor>>> GetActorByFilmId(int idFilm)
        {
            var actorByFilm = new List<Actor>();

            using (var conn = new NpgsqlConnection(_connectMovieOpinions.GetConnectMovieOpinionsDataBase()))
            {
                try
                {
                    await conn.OpenAsync();

                    await using(var getActorByFilm = new NpgsqlCommand(
                        "SELECT " +
                            "Actor_Table.id_actor, " +
                            "Actor_Table.name_actor " +
                        "FROM " +
                            "Film_Table " +
                        "LEFT JOIN " +
                            "Film_Actor_Table ON Film_Table.id_film = Film_Actor_Table.id_film " +
                        "LEFT JOIN " +
                            "Actor_Table ON Film_Actor_Table.id_actor = Actor_Table.id_actor " +
                        "WHERE " +
                            "Film_Table.id_film = @ID_FILM", conn))
                    {
                        getActorByFilm.Parameters.AddWithValue("@ID_FILM", idFilm);

                        using (var reader = await getActorByFilm.ExecuteReaderAsync())
                        {
                            while(reader.Read())
                            {
                                Actor actor = new Actor()
                                {
                                    IdActor = Convert.ToInt32(reader["id_actor"]),
                                    NameActor = reader["name_actor"].ToString()
                                };

                                actorByFilm.Add(actor);
                            }

                            if (!actorByFilm.Any())
                            {
                                return new BaseResponse<IEnumerable<Actor>>()
                                {
                                    Description = "Акторів не знайдено",
                                    StatusCode = Domain.Enum.StatusCode.NotFound
                                };
                            }

                            return new BaseResponse<IEnumerable<Actor>>()
                            {
                                Data = actorByFilm,
                                StatusCode = Domain.Enum.StatusCode.OK
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponse<IEnumerable<Actor>>()
                    {
                        Description = ex.Message,
                        StatusCode = Domain.Enum.StatusCode.InternalServerError
                    };
                }
            }
        }

        public Task<BaseResponse<Actor>> Update(Actor entity)
        {
            throw new NotImplementedException();
        }
    }
}
