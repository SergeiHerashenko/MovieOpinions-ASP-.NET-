using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MovieOpinions.server.Domain.Model.User;
using MovieOpinions.server.Service.Interfaces;
using System.Security.Claims;
using XAct.Messages;
using Microsoft.AspNetCore.Http;

namespace MovieOpinions.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel FormData)
        {
            var Response = await _accountService.Login(FormData);

            if(Response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var Token = _accountService.GenerateJwtToken(Response.Data);

                HttpContext.Response.Cookies.Append("jwt", Token, new CookieOptions
                {
                    HttpOnly = true,               
                    Secure = true,                 
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                });

                return Ok(new { user = Response.Data });
            }

            return StatusCode(
                (int)Response.StatusCode,
                new { message = Response.Description }
            );
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationModel FormData)
        {
            if(FormData.PasswordUser != FormData.ConfirmPasswordUser)
            {
                return StatusCode(422, new { message = "Паролі не співпадають!" });
            }

            var Response = await _accountService.Registartion(FormData);

            if(Response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var Token = _accountService.GenerateJwtToken(Response.Data);

                HttpContext.Response.Cookies.Append("jwt", Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                });

                return Ok(new { user = Response.Data });
            }

            return StatusCode(
                (int)Response.StatusCode,
                new { message = Response.Description }
            );
        }
    }
}
