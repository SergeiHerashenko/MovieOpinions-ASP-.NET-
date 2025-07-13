using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.DAL.Interface
{
    public interface IBaseRepository<T>
    {
        Task<BaseResponse<T>> Create(T Entity);
        Task<BaseResponse<bool>> Delete(T Entity);
        Task<BaseResponse<T>> Update(T Entity);
    }
}
