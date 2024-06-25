using FullAPIDotnet.Application.Services;
using FullAPIDotnet.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace FullAPIDotnet.Controller;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    [HttpGet]
    public IActionResult Auth(string userName, string password)
    {
        if (userName == "pedro" && password == "1234")
        {
            var token = TokenService.GenerateToken(new Employee());
            return Ok(token);
        }

        return BadRequest("Username or password invalid");
    }
}