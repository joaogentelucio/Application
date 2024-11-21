using Application.Models;

namespace Application.Repositories
{
    public interface IUsuarioRepository
    {
        Usuarios GetUsuariosByEmail(string email);

    }
}
