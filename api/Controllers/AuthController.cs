using Microsoft.AspNetCore.Mvc;
using ReferralApi.DTOs;
using ReferralApi.Services;

namespace ReferralApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromQuery] string? refCode, [FromBody] RegisterRequest request)
        {
            try
            {
                var userResponse = await _userService.RegisterAsync(request, refCode);
                var token = _jwtService.GenerateToken(await _userService.GetUserByEmailAsync(userResponse.email));

                return Ok(new
                {
                    user = userResponse,
                    token = token
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var userId = await _userService.LoginAsync(request);
                var user = await _userService.GetUserProfileAsync(int.Parse(userId));
                var token = _jwtService.GenerateToken(await _userService.GetUserByEmailAsync(user.email));

                return Ok(new
                {
                    user = user,
                    token = token
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor." });
            }
        }
    }
}