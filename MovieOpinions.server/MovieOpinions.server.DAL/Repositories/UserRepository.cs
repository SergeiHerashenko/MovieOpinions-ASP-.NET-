using MovieOpinions.server.DAL.Connect_Database;
using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Enum;
using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Domain.Response;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<BaseResponse<bool>> BlockUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<User>> Create(User Entity)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> Delete(User Entity)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<User>> GetUser(string LoginUser)
        {
            ConnectMovieOpinions ConnectDatabase = new ConnectMovieOpinions();

            using (var conn = new NpgsqlConnection(ConnectDatabase.ConnectMovieOpinionsDataBase()))
            {
                try
                {
                    await conn.OpenAsync();

                    using (var GetUserCommand = new NpgsqlCommand(
                        "SELECT " +
                            "User_Table.id_user, " +
                            "User_Table.login_user, " +
                            "User_Table.email_user, " +
                            "User_Table.role_user, " +
                            "" +
                            "User_Profile_Table.firstname_user, " +
                            "User_Profile_Table.lastname_user, " +
                            "User_Profile_Table.bio_user, " +
                            "User_Profile_Table.avatar_user, " +
                            "User_Profile_Table.created_at, " +
                            "User_Profile_Table.update_at, " +
                            "" +
                            "User_Security_Table.password_hash_user, " +
                            "User_Security_Table.password_salt_user, " +
                            "User_Security_Table.failed_login_attempts, " +
                            "User_Security_Table.is_blocked, " +
                            "User_Security_Table.is_deleted, " +
                            "User_Security_Table.email_confirmed, " +
                            "User_Security_Table.last_login " +
                        "FROM " +
                            "User_Table " +
                        "JOIN " +
                            "User_Profile_Table ON User_Table.id_user = User_Profile_Table.id_user " +
                        "JOIN " +
                            "User_Security_Table ON User_Table.id_user = User_Security_Table.id_user " +
                        "WHERE " +
                            "User_Table.login_user = @LoginUser", conn))
                    {
                        GetUserCommand.Parameters.AddWithValue("@LoginUser", LoginUser);

                        using (var ReaderInformationUser = await GetUserCommand.ExecuteReaderAsync())
                        {
                            if (ReaderInformationUser.Read())
                            {
                                User user = new User
                                {
                                    UserId = Guid.Parse(ReaderInformationUser["id_user"].ToString()),
                                    LoginUser = ReaderInformationUser["login_user"].ToString(),
                                    EmailUser = ReaderInformationUser["email_user"].ToString(),
                                    Role = (Role)Convert.ToInt32(ReaderInformationUser["role_user"]),

                                    Profile = new UserProfile
                                    {
                                        FirstName = ReaderInformationUser["firstname_user"].ToString(),
                                        LastName = ReaderInformationUser["lastname_user"].ToString(),
                                        Bio = ReaderInformationUser["bio_user"].ToString(),
                                        AvatarUrl = ReaderInformationUser["avatar_user"].ToString(),
                                        CreatedAt = Convert.ToDateTime(ReaderInformationUser["created_at"]),
                                        UpdatedAt = ReaderInformationUser["update_at"] == DBNull.Value
                                            ? DateTime.MinValue
                                            : Convert.ToDateTime(ReaderInformationUser["update_at"].ToString()),
                                    },

                                    Security = new UserSecurity
                                    {
                                        PasswordHash = ReaderInformationUser["password_hash_user"].ToString(),
                                        PasswordSalt = ReaderInformationUser["password_salt_user"].ToString(),
                                        FailedLoginAttempts = Convert.ToInt32(ReaderInformationUser["failed_login_attempts"].ToString()),
                                        IsBlocked = Convert.ToBoolean(ReaderInformationUser["is_blocked"]),
                                        IsDeleted = Convert.ToBoolean(ReaderInformationUser["is_deleted"]),
                                        IsEmailConfirmed = Convert.ToBoolean(ReaderInformationUser["email_confirmed"]),
                                        LastLoginDate = ReaderInformationUser["last_login"] == DBNull.Value
                                            ? DateTime.MinValue
                                            : Convert.ToDateTime(ReaderInformationUser["last_login"].ToString()),
                                    }
                                };

                                return new BaseResponse<User>()
                                {
                                    Data = user,
                                    Description = "Користувач знайдений!",
                                    StatusCode = Domain.Enum.StatusCode.OK
                                };
                            }
                        }
                    }
                    
                    return new BaseResponse<User>
                    {
                        StatusCode = Domain.Enum.StatusCode.NotFound,
                        Description = "Користувача не знайдено!",
                        Data = null
                    };
                }
                catch (Exception ex) 
                {
                    return new BaseResponse<User>
                    {
                        StatusCode = Domain.Enum.StatusCode.InternalServerError,
                        Description = ex.Message
                    };
                }
            }
        }

        public Task<BaseResponse<User>> GetUserId(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<User>> Update(User Entity)
        {
            throw new NotImplementedException();
        }
    }
}
