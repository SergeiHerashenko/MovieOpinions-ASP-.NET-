using Microsoft.AspNetCore.Mvc;
using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Service.Implementations;
using MovieOpinions.server.Service.Interfaces;

namespace MovieOpinions.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmController : Controller
    {
        private readonly IFilmService _filmService;

        public FilmController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        [HttpGet("films")]
        public async Task<IActionResult> GetFilms()
        {
            var response = await _filmService.GetAllFilms();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(new { films = response.Data });
            }

            return StatusCode(
                (int)response.StatusCode,
                new { message = response.Description }
            );
        }
    }
}
