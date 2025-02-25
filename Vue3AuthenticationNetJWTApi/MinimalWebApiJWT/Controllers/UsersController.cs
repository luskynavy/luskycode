using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimalWebApiJWT.Models;
using MinimalWebApiJWT.Services;

namespace MinimalWebApiJWT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            User? user = _userService.FindByUsername(request.Username);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            if (user.Password != request.Password)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            // Generate token
            var token = _userService.CreateToken(user);

            // Return the token
            return Ok(new AuthResponse(user, token));
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
