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
        public async Task<IActionResult> Login([FromBody] LoginModel formData)
        {
            var response = await _accountService.Login(formData);

            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var token = _accountService.GenerateJwtToken(response.Data);

                SetJwtCookie(token);

                return Ok(new { user = response.Data });
            }

            return StatusCode(
                (int)response.StatusCode,
                new { message = response.Description }
            );
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationModel formData)
        {
            if(formData.PasswordUser != formData.ConfirmPasswordUser)
            {
                return StatusCode(422, new { message = "Паролі не співпадають!" });
            }

            var response = await _accountService.Registration(formData);

            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var token = _accountService.GenerateJwtToken(response.Data);

                SetJwtCookie(token);

                return Ok(new { user = response.Data });
            }

            return StatusCode(
                (int)response.StatusCode,
                new { message = response.Description }
            );
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("jwt");

            return Ok(new { message = "Ви вийшли з системи" });
        }


        private void SetJwtCookie(string token)
        {
            HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });
        }
    }
}
