using Microsoft.AspNetCore.Mvc;
using MovieOpinions.server.Service.Interfaces;

namespace MovieOpinions.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllCommentFilm([FromRoute(Name = "id")] int idFilm)
        {
            var response = await _commentService.GetAllCommentFilm(idFilm);

            return Ok(response);
        }
    }
}
