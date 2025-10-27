using Microsoft.AspNetCore.Http;
using MovieOpinions.server.Domain.Enum;
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
    public class MediaService : IMediaService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MediaService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<ImageDTO>> GetBackground(string namePage)
        {
            var response = new BaseResponse<ImageDTO>();

            try
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Background");

                if (!Directory.Exists(folderPath))
                {
                    response.Description = "Images folder not found";
                    response.StatusCode = Domain.Enum.StatusCode.NotFound;
                    return response;
                }

                var request = _httpContextAccessor.HttpContext?.Request;
                var baseUrl = $"{request?.Scheme}://{request?.Host}/Image/Background/";

                string fileName;
                if (namePage != "FilmPage")
                    fileName = "Background_Image.png";
                else
                    fileName = "Background_Image_2.png";

                var filePath = Path.Combine(folderPath, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    response.Description = "File not found";
                    response.StatusCode = Domain.Enum.StatusCode.NotFound;
                    return response;
                }

                response.Data = new ImageDTO
                {
                    Id = 0,
                    Src = baseUrl + fileName,
                    Alt = Path.GetFileNameWithoutExtension(fileName)
                };

                response.StatusCode = Domain.Enum.StatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                response.Description = $"[GetBackground] : {ex.Message}";
                response.StatusCode = StatusCode.InternalServerError;
                return response;
            }
        }

        public async Task<BaseResponse<ImageDTO>> GetHomeIcon()
        {
            var response = new BaseResponse<ImageDTO>();

            try
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Icons");

                if (!Directory.Exists(folderPath))
                {
                    response.Description = "Images folder not found";
                    response.StatusCode = Domain.Enum.StatusCode.NotFound;
                    return response;
                }

                var request = _httpContextAccessor.HttpContext?.Request;
                var baseUrl = $"{request?.Scheme}://{request?.Host}/Image/Icons/";

                string fileName = "Login_icon.png";
                
                var filePath = Path.Combine(folderPath, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    response.Description = "File not found";
                    response.StatusCode = Domain.Enum.StatusCode.NotFound;
                    return response;
                }

                response.Data = new ImageDTO
                {
                    Id = 0,
                    Src = baseUrl + fileName,
                    Alt = Path.GetFileNameWithoutExtension(fileName)
                };

                response.StatusCode = Domain.Enum.StatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                response.Description = $"[GetBackground] : {ex.Message}";
                response.StatusCode = StatusCode.InternalServerError;
                return response;
            }
        }

        public async Task<BaseResponse<List<ImageDTO>>> GetHomeImages()
        {
            var response = new BaseResponse<List<ImageDTO>>();

            try
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "HomePage");

                if (!Directory.Exists(folderPath))
                {
                    response.Description = "Images folder not found";
                    response.StatusCode = Domain.Enum.StatusCode.NotFound;
                    return response;
                }

                var request = _httpContextAccessor.HttpContext?.Request;
                var baseUrl = $"{request?.Scheme}://{request?.Host}/Image/HomePage/";

                var files = Directory.GetFiles(folderPath)
                    .Select((file, index) => new ImageDTO
                    {
                        Id = index,
                        Src = baseUrl + Path.GetFileName(file),
                        Alt = Path.GetFileNameWithoutExtension(file)
                    })
                    .ToList();

                response.Data = files;
                response.StatusCode = Domain.Enum.StatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                response.Description = $"[GetHomeImages] : {ex.Message}";
                response.StatusCode = StatusCode.InternalServerError;
                return response;
            }
        }
    }
}
