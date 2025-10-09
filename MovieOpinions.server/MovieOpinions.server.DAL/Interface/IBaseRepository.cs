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
        Task<BaseResponse<T>> Create(T entity);

        Task<BaseResponse<bool>> Delete(T entity);

        Task<BaseResponse<T>> Update(T entity);
    }
}
