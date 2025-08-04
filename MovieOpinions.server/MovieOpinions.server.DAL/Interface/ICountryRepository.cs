using MovieOpinions.server.Domain.Model;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.DAL.Interface
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
        Task<BaseResponse<IEnumerable<Country>>> GetCountryMovie(int IdFilm);
    }
}
