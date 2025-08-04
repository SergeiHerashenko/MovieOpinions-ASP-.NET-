using MovieOpinions.server.DAL.Connect_Database;
using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model;
using MovieOpinions.server.Domain.Response;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.DAL.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IConnectMovieOpinions _connectMovieOpinions;

        public CountryRepository(IConnectMovieOpinions connectMovieOpinions)
        {
            _connectMovieOpinions = connectMovieOpinions;
        }

        public Task<BaseResponse<Country>> Create(Country entity)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> Delete(Country entity)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IEnumerable<Country>>> GetCountryMovie(int IdFilm)
        {
            var filmCountry = new List<Country>();

            using(var conn = new NpgsqlConnection(_connectMovieOpinions.GetConnectMovieOpinionsDataBase()))
            {
                try
                {
                    await conn.OpenAsync();

                    await using(var getCountryFilm = new NpgsqlCommand(
                        "SELECT " +
                            "Country_Table.id_country, " +
                            "Country_Table.name_country " +
                        "FROM " +
                            "Film_Table " +
                        "LEFT JOIN " +
                            "Film_Country_Table ON Film_Table.id_film = Film_Country_Table.id_film " +
                        "LEFT JOIN " +
                            "Country_Table ON Film_Country_Table.id_country = Country_Table.id_country " +
                        "WHERE " +
                            "Film_Table.id_film = @ID_FILM", conn))
                    {
                        getCountryFilm.Parameters.AddWithValue("@ID_FILM", IdFilm);

                        using (var reader = await getCountryFilm.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Country country = new Country()
                                {
                                    IdCountry = Convert.ToInt32(reader["id_country"]),
                                    NameCountry = reader["name_country"].ToString()
                                };

                                filmCountry.Add(country);
                            }

                            if (!filmCountry.Any())
                            {
                                return new BaseResponse<IEnumerable<Country>>()
                                {
                                    Description = "Країн не знайдено",
                                    StatusCode = Domain.Enum.StatusCode.NotFound
                                };
                            }

                            return new BaseResponse<IEnumerable<Country>>()
                            {
                                Data = filmCountry,
                                StatusCode = Domain.Enum.StatusCode.OK
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponse<IEnumerable<Country>>()
                    {
                        Description = ex.Message,
                        StatusCode = Domain.Enum.StatusCode.InternalServerError
                    };
                }
            }
        }

        public Task<BaseResponse<Country>> Update(Country entity)
        {
            throw new NotImplementedException();
        }
    }
}
