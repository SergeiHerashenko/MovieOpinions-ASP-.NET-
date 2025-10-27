using Microsoft.AspNetCore.Mvc;
using MovieOpinions.server.Service.Interfaces;

namespace MovieOpinions.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet("background/{pageName}")]
        public async Task<IActionResult> GetBackground(string pageName)
        {
            var images = await _mediaService.GetBackground(pageName);
            return Ok(images);
        }
    }
}
