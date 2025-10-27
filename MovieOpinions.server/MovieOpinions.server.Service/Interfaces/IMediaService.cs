using MovieOpinions.server.Domain.Model;
using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Service.Interfaces
{
    public interface IMediaService
    {
        Task<BaseResponse<List<ImageDTO>>> GetHomeImages();

        Task<BaseResponse<ImageDTO>> GetBackground(string namePage);

        Task<BaseResponse<ImageDTO>> GetHomeIcon();
    }
}
