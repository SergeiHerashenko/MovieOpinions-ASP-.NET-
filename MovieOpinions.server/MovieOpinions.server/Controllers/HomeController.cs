using Microsoft.AspNetCore.Mvc;
using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Service.Implementations;
using MovieOpinions.server.Service.Interfaces;
using System.Threading.Tasks;

namespace MovieOpinions.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public HomeController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet("image")]
        public async Task<IActionResult> GetHomeImages()
        {
            var images = await _mediaService.GetHomeImages();
            return Ok(images);
        }

        [HttpGet("icon")]
        public async Task<IActionResult> GetHomeIcon()
        {
            var icon = await _mediaService.GetHomeIcon();
            return Ok(icon);
        }
    }
}
