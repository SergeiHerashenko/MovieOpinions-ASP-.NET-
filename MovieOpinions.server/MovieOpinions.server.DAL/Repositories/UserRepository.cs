using MovieOpinions.server.DAL.Connect_Database;
using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Enum;
using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Domain.Response;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MovieOpinions.server.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConnectMovieOpinions _connectMovieOpinions;

        public UserRepository(IConnectMovieOpinions connectMovieOpinions)
        {
            _connectMovieOpinions = connectMovieOpinions;
        }

        public Task<BaseResponse<bool>> BlockUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<User>> Create(User Entity)
        {
            using (var conn = new NpgsqlConnection(_connectMovieOpinions.ConnectMovieOpinionsDataBase()))
            {
                try
                {
                    await conn.OpenAsync();

                    await using (var Transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            await InsertUserTableAsync(conn, Transaction, Entity);
                            await InsertUserProfileTableAsync(conn, Transaction, Entity);
                            await InsertUserSecurityTableAsync(conn, Transaction, Entity);

                            await Transaction.CommitAsync();

                            return new BaseResponse<User>()
                            {
                                Data = Entity,
                                Description = "Користувач створений!",
                                StatusCode = Domain.Enum.StatusCode.OK
                            };
                        }
                        catch (Exception ex)
                        {
                            await Transaction.RollbackAsync();

                            return new BaseResponse<User>()
                            {
                                Data = null,
                                Description = "Помилка при створенні користувача!" + ex.Message,
                                StatusCode = Domain.Enum.StatusCode.InternalServerError
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponse<User>()
                    {
                        Data = null,
                        Description = "Помилка з базою данних!" + ex.Message,
                        StatusCode = Domain.Enum.StatusCode.InternalServerError
                    };
                }
            }
        }

        public Task<BaseResponse<bool>> Delete(User Entity)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<User>> GetUser(string LoginUser)
        {
            using (var conn = new NpgsqlConnection(_connectMovieOpinions.ConnectMovieOpinionsDataBase()))
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

        private async Task InsertUserTableAsync(NpgsqlConnection conn, NpgsqlTransaction Transaction, User Entity)
        {
            var InsertUserTable = new NpgsqlCommand(
                                "INSERT INTO " +
                                    "User_Table (id_user, login_user, email_user, role_user) " +
                                "VALUES (@Id, @Login, @Email, @Role);", conn, Transaction);

            InsertUserTable.Parameters.AddWithValue("@Id", Entity.UserId);
            InsertUserTable.Parameters.AddWithValue("@Login", Entity.LoginUser);
            InsertUserTable.Parameters.Add("@Email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = (object?)Entity.EmailUser ?? DBNull.Value;
            InsertUserTable.Parameters.AddWithValue("@Role", (int)Entity.Role);

            await InsertUserTable.ExecuteNonQueryAsync();
        }

        private async Task InsertUserProfileTableAsync(NpgsqlConnection conn, NpgsqlTransaction Transaction, User Entity)
        {
            var InsertUserProfileTable = new NpgsqlCommand(
                                "INSERT INTO " +
                                    "User_Profile_Table (id_user, firstname_user, lastname_user, bio_user, avatar_user, created_at, update_at) " +
                                "VALUES (@Id, @FirstName, @LastName, @Bio, @Avatar, @CreatedAt, @UpdateAt);", conn, Transaction);

            InsertUserProfileTable.Parameters.AddWithValue("@Id", Entity.UserId);
            InsertUserProfileTable.Parameters.Add("@FirstName", NpgsqlTypes.NpgsqlDbType.Varchar).Value = (object?)Entity.Profile.FirstName ?? DBNull.Value;
            InsertUserProfileTable.Parameters.Add("@LastName", NpgsqlTypes.NpgsqlDbType.Varchar).Value = (object?)Entity.Profile.LastName ?? DBNull.Value;
            InsertUserProfileTable.Parameters.Add("@Bio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = (object?)Entity.Profile.Bio ?? DBNull.Value;
            InsertUserProfileTable.Parameters.Add("@Avatar", NpgsqlTypes.NpgsqlDbType.Varchar).Value = (object?)Entity.Profile.AvatarUrl ?? DBNull.Value;
            InsertUserProfileTable.Parameters.Add("@CreatedAt", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = (object?)Entity.Profile.CreatedAt ?? DBNull.Value;
            InsertUserProfileTable.Parameters.Add("@UpdateAt", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = (object?)Entity.Profile.UpdatedAt ?? DBNull.Value;

            await InsertUserProfileTable.ExecuteNonQueryAsync();
        }

        private async Task InsertUserSecurityTableAsync(NpgsqlConnection conn, NpgsqlTransaction Transaction, User Entity)
        {
            var InsertUserSecurityTable = new NpgsqlCommand(
                                "INSERT INTO " +
                                    "User_Security_Table " +
                                    "(id_user, password_hash_user, password_salt_user, failed_login_attempts, is_blocked, is_deleted, email_confirmed, last_login) " +
                                "VALUES (@Id, @PasswordHash, @PasswordSalt, @FailedLogin, @IsBlocked, @IsDeleted, @EmailConfirmed, @LastLogin);", conn, Transaction);

            InsertUserSecurityTable.Parameters.AddWithValue("@Id", Entity.UserId);
            InsertUserSecurityTable.Parameters.AddWithValue("@PasswordHash", Entity.Security.PasswordHash);
            InsertUserSecurityTable.Parameters.AddWithValue("@PasswordSalt", Entity.Security.PasswordSalt);
            InsertUserSecurityTable.Parameters.AddWithValue("@FailedLogin", Entity.Security.FailedLoginAttempts);
            InsertUserSecurityTable.Parameters.AddWithValue("@IsBlocked", Entity.Security.IsBlocked);
            InsertUserSecurityTable.Parameters.AddWithValue("@IsDeleted", Entity.Security.IsDeleted);
            InsertUserSecurityTable.Parameters.AddWithValue("@EmailConfirmed", Entity.Security.IsEmailConfirmed);
            InsertUserSecurityTable.Parameters.Add("@LastLogin", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = (object?)Entity.Security.LastLoginDate ?? DBNull.Value;

            await InsertUserSecurityTable.ExecuteNonQueryAsync();
        }
    }
}
