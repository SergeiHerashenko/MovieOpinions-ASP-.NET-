using MovieOpinions.server.DAL.Interface;
using MovieOpinions.server.Domain.Model;
using MovieOpinions.server.Domain.Response;
using MovieOpinions.server.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Implementations
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService (ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }   

        public async Task<BaseResponse<IEnumerable<Country>>> GetCountryByFilm(int idFilm)
        {
            var responsr = await _countryRepository.GetCountryMovie(idFilm);

            if(responsr.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return new BaseResponse<IEnumerable<Country>>()
                {
                    Description = responsr.Description,
                    StatusCode = responsr.StatusCode,
                };
            }

            return new BaseResponse<IEnumerable<Country>>()
            {
                Data = responsr.Data,
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
    }
}
