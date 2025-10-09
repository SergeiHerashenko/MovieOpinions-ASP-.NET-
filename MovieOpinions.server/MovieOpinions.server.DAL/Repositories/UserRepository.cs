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

        public Task<BaseResponse<bool>> BlockUser(UserEntity user)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<UserEntity>> Create(UserEntity entity)
        {
            using (var conn = new NpgsqlConnection(_connectMovieOpinions.GetConnectMovieOpinionsDataBase()))
            {
                try
                {
                    await conn.OpenAsync();

                    await using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            await InsertUserTableAsync(conn, transaction, entity);
                            await InsertUserProfileTableAsync(conn, transaction, entity);
                            await InsertUserSecurityTableAsync(conn, transaction, entity);

                            await transaction.CommitAsync();

                            return new BaseResponse<UserEntity>()
                            {
                                Data = entity,
                                Description = "Користувач створений!",
                                StatusCode = Domain.Enum.StatusCode.OK
                            };
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();

                            return new BaseResponse<UserEntity>()
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
                    return new BaseResponse<UserEntity>()
                    {
                        Data = null,
                        Description = "Помилка з базою данних!" + ex.Message,
                        StatusCode = Domain.Enum.StatusCode.InternalServerError
                    };
                }
            }
        }

        public Task<BaseResponse<bool>> Delete(UserEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<UserEntity>> GetUser(string loginUser)
        {
            using (var conn = new NpgsqlConnection(_connectMovieOpinions.GetConnectMovieOpinionsDataBase()))
            {
                try
                {
                    await conn.OpenAsync();

                    using (var getUserCommand = new NpgsqlCommand(
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
                        getUserCommand.Parameters.AddWithValue("@LoginUser", loginUser);

                        using (var readerInformationUser = await getUserCommand.ExecuteReaderAsync())
                        {
                            if (readerInformationUser.Read())
                            {
                                var user = MapReaderToUser(readerInformationUser);

                                return new BaseResponse<UserEntity>()
                                {
                                    Data = user,
                                    Description = "Користувач знайдений!",
                                    StatusCode = Domain.Enum.StatusCode.OK
                                };
                            }
                        }
                    }
                    
                    return new BaseResponse<UserEntity>
                    {
                        StatusCode = Domain.Enum.StatusCode.NotFound,
                        Description = "Користувача не знайдено!",
                        Data = null
                    };
                }
                catch (Exception ex) 
                {
                    return new BaseResponse<UserEntity>
                    {
                        StatusCode = Domain.Enum.StatusCode.InternalServerError,
                        Description = ex.Message
                    };
                }
            }
        }

        public async Task<BaseResponse<UserEntity>> GetUserId(Guid idUser)
        {
            using (var conn = new NpgsqlConnection(_connectMovieOpinions.GetConnectMovieOpinionsDataBase()))
            {
                try
                {
                    await conn.OpenAsync();

                    using (var getUserById = new NpgsqlCommand(
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
                            "User_Table.id_user = @ID_USER", conn))
                    {
                        getUserById.Parameters.AddWithValue("@ID_USER", idUser);

                        using (var readerInformationUser = await getUserById.ExecuteReaderAsync())
                        {
                            if (readerInformationUser.Read())
                            {
                                var user = MapReaderToUser(readerInformationUser);

                                return new BaseResponse<UserEntity>()
                                {
                                    Data = user,
                                    Description = "Користувач знайдений!",
                                    StatusCode = Domain.Enum.StatusCode.OK
                                };
                            }
                        }
                    }

                    return new BaseResponse<UserEntity>
                    {
                        StatusCode = Domain.Enum.StatusCode.NotFound,
                        Description = "Користувача не знайдено!",
                        Data = null
                    };
                }
                catch (Exception ex)
                {
                    return new BaseResponse<UserEntity>
                    {
                        StatusCode = Domain.Enum.StatusCode.InternalServerError,
                        Description = ex.Message
                    };
                }
            }
        }

        public Task<BaseResponse<UserEntity>> Update(UserEntity entity)
        {
            throw new NotImplementedException();
        }

        private async Task InsertUserTableAsync(NpgsqlConnection conn, NpgsqlTransaction transaction, UserEntity entity)
        {
            var insertUserTable = new NpgsqlCommand(
                                "INSERT INTO " +
                                    "User_Table (id_user, login_user, email_user, role_user) " +
                                "VALUES (@Id, @Login, @Email, @Role);", conn, transaction);

            insertUserTable.Parameters.AddWithValue("@Id", entity.UserId);
            insertUserTable.Parameters.AddWithValue("@Login", entity.LoginUser);
            insertUserTable.Parameters.Add("@Email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = (object?)entity.EmailUser ?? DBNull.Value;
            insertUserTable.Parameters.AddWithValue("@Role", (int)entity.Role);

            await insertUserTable.ExecuteNonQueryAsync();
        }

        private async Task InsertUserProfileTableAsync(NpgsqlConnection conn, NpgsqlTransaction transaction, UserEntity entity)
        {
            var insertUserProfileTable = new NpgsqlCommand(
                                "INSERT INTO " +
                                    "User_Profile_Table (id_user, firstname_user, lastname_user, bio_user, avatar_user, created_at, update_at) " +
                                "VALUES (@Id, @FirstName, @LastName, @Bio, @Avatar, @CreatedAt, @UpdateAt);", conn, transaction);

            insertUserProfileTable.Parameters.AddWithValue("@Id", entity.UserId);
            insertUserProfileTable.Parameters.Add("@FirstName", NpgsqlTypes.NpgsqlDbType.Varchar).Value = (object?)entity.Profile.FirstName ?? DBNull.Value;
            insertUserProfileTable.Parameters.Add("@LastName", NpgsqlTypes.NpgsqlDbType.Varchar).Value = (object?)entity.Profile.LastName ?? DBNull.Value;
            insertUserProfileTable.Parameters.Add("@Bio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = (object?)entity.Profile.Bio ?? DBNull.Value;
            insertUserProfileTable.Parameters.Add("@Avatar", NpgsqlTypes.NpgsqlDbType.Varchar).Value = (object?)entity.Profile.AvatarUrl ?? DBNull.Value;
            insertUserProfileTable.Parameters.Add("@CreatedAt", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = (object?)entity.Profile.CreatedAt ?? DBNull.Value;
            insertUserProfileTable.Parameters.Add("@UpdateAt", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = (object?)entity.Profile.UpdatedAt ?? DBNull.Value;

            await insertUserProfileTable.ExecuteNonQueryAsync();
        }

        private async Task InsertUserSecurityTableAsync(NpgsqlConnection conn, NpgsqlTransaction transaction, UserEntity entity)
        {
            var insertUserSecurityTable = new NpgsqlCommand(
                                "INSERT INTO " +
                                    "User_Security_Table " +
                                    "(id_user, password_hash_user, password_salt_user, failed_login_attempts, is_blocked, is_deleted, email_confirmed, last_login) " +
                                "VALUES (@Id, @PasswordHash, @PasswordSalt, @FailedLogin, @IsBlocked, @IsDeleted, @EmailConfirmed, @LastLogin);", conn, transaction);

            insertUserSecurityTable.Parameters.AddWithValue("@Id", entity.UserId);
            insertUserSecurityTable.Parameters.AddWithValue("@PasswordHash", entity.Security.PasswordHash);
            insertUserSecurityTable.Parameters.AddWithValue("@PasswordSalt", entity.Security.PasswordSalt);
            insertUserSecurityTable.Parameters.AddWithValue("@FailedLogin", entity.Security.FailedLoginAttempts);
            insertUserSecurityTable.Parameters.AddWithValue("@IsBlocked", entity.Security.IsBlocked);
            insertUserSecurityTable.Parameters.AddWithValue("@IsDeleted", entity.Security.IsDeleted);
            insertUserSecurityTable.Parameters.AddWithValue("@EmailConfirmed", entity.Security.IsEmailConfirmed);
            insertUserSecurityTable.Parameters.Add("@LastLogin", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = (object?)entity.Security.LastLoginDate ?? DBNull.Value;

            await insertUserSecurityTable.ExecuteNonQueryAsync();
        }

        private UserEntity MapReaderToUser(NpgsqlDataReader reader)
        {
            return new UserEntity
            {
                UserId = Guid.Parse(reader["id_user"].ToString()),
                LoginUser = reader["login_user"].ToString(),
                EmailUser = reader["email_user"].ToString(),
                Role = (Role)Convert.ToInt32(reader["role_user"]),
                Profile = new UserProfile
                {
                    FirstName = reader["firstname_user"].ToString(),
                    LastName = reader["lastname_user"].ToString(),
                    Bio = reader["bio_user"].ToString(),
                    AvatarUrl = reader["avatar_user"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                    UpdatedAt = reader["update_at"] == DBNull.Value
                ? DateTime.MinValue
                : Convert.ToDateTime(reader["update_at"].ToString())
                },
                Security = new UserSecurity
                {
                    PasswordHash = reader["password_hash_user"].ToString(),
                    PasswordSalt = reader["password_salt_user"].ToString(),
                    FailedLoginAttempts = Convert.ToInt32(reader["failed_login_attempts"].ToString()),
                    IsBlocked = Convert.ToBoolean(reader["is_blocked"]),
                    IsDeleted = Convert.ToBoolean(reader["is_deleted"]),
                    IsEmailConfirmed = Convert.ToBoolean(reader["email_confirmed"]),
                    LastLoginDate = reader["last_login"] == DBNull.Value
                ? DateTime.MinValue
                : Convert.ToDateTime(reader["last_login"].ToString())
                }
            };
        }
    }
}
