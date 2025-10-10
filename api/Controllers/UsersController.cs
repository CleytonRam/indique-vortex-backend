using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReferralApi.Services;

namespace ReferralApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase 
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UsersController(IUserService userService, IJwtService jwtService) 
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                if (!User.Identity?.IsAuthenticated ?? true)
                    return Unauthorized(new { message = "Não autenticado." });

                var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdStr))
                    return Unauthorized(new { message = "Token sem NameIdentifier." });

                if (!int.TryParse(userIdStr, out var userId))
                    return Unauthorized(new { message = "Claim NameIdentifier inválida." });

                var user = await _userService.GetUserProfileAsync(userId);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno do servidor." });
            }
        }

    }
}