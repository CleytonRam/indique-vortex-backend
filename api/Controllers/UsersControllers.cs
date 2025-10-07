using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReferralApi.Services;

namespace ReferralApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersControllers : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UsersControllers(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var userId = _jwtService.ValidateToken(token);

                if (userId == null)
                    return Unauthorized(new { message = "Token inválido." });

                var user = await _userService.GetUserProfileAsync(userId.Value);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor." });
            }
        }
    }
}