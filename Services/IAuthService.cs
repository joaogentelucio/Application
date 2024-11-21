using Application.Models;

namespace Application.Services
{
    public interface IAuthService
    {
        Usuarios Authenticate(string email, string senha);
        string GenerateJwtToken(Usuarios usuarios);
    }
}
