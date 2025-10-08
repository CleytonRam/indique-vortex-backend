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
                Console.WriteLine($"📥 Registro recebido: {request.email}, RefCode: {refCode}");

                var userResponse = await _userService.RegisterAsync(request, refCode);

                // ✅ CORREÇÃO: Buscar a entidade User pelo ID do usuário registrado
                var userEntity = await _userService.GetUserByIdAsync(userResponse.id);
                var token = _jwtService.GenerateToken(userEntity);

                Console.WriteLine($"✅ Usuário registrado: {userResponse.email}");
                Console.WriteLine($"🎯 RefCode gerado: {userResponse.refCode}");
                Console.WriteLine($"📊 Pontos: {userResponse.points}");

                return Ok(new
                {
                    user = userResponse,
                    token = token
                });
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"❌ Erro no registro: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Erro interno: {ex}");
                return StatusCode(500, new { message = "Erro interno do servidor." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                Console.WriteLine($"🔐 Tentando login: {request.email}");

                var userId = await _userService.LoginAsync(request);

                // ✅ CORREÇÃO: Buscar o usuário pelo ID retornado, não pelo email
                var user = await _userService.GetUserProfileAsync(int.Parse(userId));

                Console.WriteLine($"✅ Login bem-sucedido - UserId: {userId}");

                // ✅ CORREÇÃO: Buscar a entidade User pelo ID para gerar o token
                var userEntity = await _userService.GetUserByIdAsync(int.Parse(userId));
                var token = _jwtService.GenerateToken(userEntity);

                Console.WriteLine($"🎫 Token gerado: {token?.Substring(0, Math.Min(20, token.Length))}...");
                Console.WriteLine($"👤 Dados do usuário: {user.name}, {user.email}, Pontos: {user.points}");

                return Ok(new
                {
                    user = user,
                    token = token
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"❌ Credenciais inválidas: {ex.Message}");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Erro interno no login: {ex}");
                return StatusCode(500, new { message = "Erro interno do servidor." });
            }
        }
    }
}