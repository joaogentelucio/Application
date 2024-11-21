using Application.Helpers;
using Application.Models;
using Application.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly string _secretKey = "292b6158-d54b-4e97-8acc-6d0a9e54a1c8";

        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public Usuarios Authenticate(string email, string senha)
        {
            var usuario = _usuarioRepository.GetUsuariosByEmail(email);

            if (!PasswordHasher.VerifyPassword(senha, usuario.Senha))
                return null;

            return usuario;
        }

        public string GenerateJwtToken(Usuarios usuarios)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuarios.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuarios.Nome),
                    new Claim(ClaimTypes.Email, usuarios.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
