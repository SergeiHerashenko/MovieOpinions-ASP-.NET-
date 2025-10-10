using MovieOpinions.server.DAL.Connect_Database;
using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model.Comments;
using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Domain.Response;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MovieOpinions.server.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IConnectMovieOpinions _connectMovieOpinions;

        public CommentRepository (IConnectMovieOpinions connectMovieOpinions)
        {
            _connectMovieOpinions = connectMovieOpinions;
        }

        public Task<BaseResponse<Comment>> Create(Comment entity)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> Delete(Comment entity)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IEnumerable<Comment>>> GetCommentFilm(int idFilm)
        {
            var commentByFilm = new List<Comment>();

            using (var conn = new NpgsqlConnection(_connectMovieOpinions.GetConnectMovieOpinionsDataBase()))
            {
                try
                {
                    await conn.OpenAsync();

                    await using (var getAllComment = new NpgsqlCommand(
                        "SELECT " +
                            "id_comment, id_user, id_film, text_comment, parent_comment_id, create_at, is_deleted, is_edited " +
                        "FROM " +
                            "Comment_Film_Table " +
                        "WHERE " +
                            "id_film = @ID_FILM", conn))
                    {
                        getAllComment.Parameters.AddWithValue("@ID_FILM", idFilm);

                        using (var reader = await getAllComment.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Comment comment = new Comment()
                                {
                                    IdComment = Convert.ToInt32(reader["id_comment"]),
                                    IdUser = Guid.Parse(reader["id_user"].ToString()),
                                    IdFilm = Convert.ToInt32(reader["id_film"]),
                                    TextComment = reader["text_comment"].ToString(),
                                    IsDeleted = Convert.ToBoolean(reader["is_deleted"]),
                                    IsEdited = Convert.ToBoolean(reader["is_edited"]),
                                    CreatedAt = reader["create_at"] == DBNull.Value
                                            ? DateTime.MinValue
                                            : Convert.ToDateTime(reader["create_at"].ToString()),
                                    ParentCommentId = reader["parent_comment_id"] == DBNull.Value
                                            ? (int?)null
                                            : Convert.ToInt32(reader["parent_comment_id"]),
                                    User = new UserProfile()
                                };

                                commentByFilm.Add(comment);
                            }
                        }

                        if (!commentByFilm.Any())
                        {
                            return new BaseResponse<IEnumerable<Comment>>()
                            {
                                StatusCode = Domain.Enum.StatusCode.NotFound,
                                Description = "Коментарів не знайдено"
                            };
                        }

                        return new BaseResponse<IEnumerable<Comment>>()
                        {
                            StatusCode = Domain.Enum.StatusCode.OK,
                            Data = commentByFilm
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponse<IEnumerable<Comment>>
                    {
                        StatusCode = Domain.Enum.StatusCode.InternalServerError,
                        Description = ex.Message
                    };
                }
            }
        }

        public Task<BaseResponse<Comment>> Update(Comment entity)
        {
            throw new NotImplementedException();
        }
    }
}
