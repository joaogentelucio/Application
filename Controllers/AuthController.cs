using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var usuario = _authService.Authenticate(request.Email, request.Senha);

            if (usuario == null)
                return Unauthorized(new { message = "Email ou senha inválidos" });

            var token = _authService.GenerateJwtToken(usuario);

            return Ok(new
            {
                message = "Login realizado com sucesso",
                token,
                usuario = new { usuario.Id, usuario.Nome, usuario.Email }
            });
        }
    }
}
