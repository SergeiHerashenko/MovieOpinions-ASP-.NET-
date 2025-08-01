using Microsoft.AspNetCore.Mvc;
using MovieOpinions.server.Service.Interfaces;

namespace MovieOpinions.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet("genre")]
        public async Task<IActionResult> GetGenre()
        {
            var response = await _genreService.GetAllGenre();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(new { genres = response.Data });
            }

            return StatusCode(
                (int)response.StatusCode,
                new { message = response.Description }
            );
        }
    }
}
