using Application.Helpers;
using Application.Models;
using Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {

        private readonly UsuariosRepository _usuariosRepository;
        public UsuariosController(UsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        // METODO PARA INSERIR UM NOVO USUARIO NO BANCO DADOS
        [HttpPost("InserirUsuario")]
        public IActionResult InserirUsuario([FromBody] Usuarios cadastro)
        {
            if (cadastro == null)
                return BadRequest("Dados do usuário inválidos.");

            try
            {
                // Hasheando a senha do usuário antes de salvar
                cadastro.Senha = PasswordHasher.HashPassword(cadastro.Senha);

                _usuariosRepository.Salvar(cadastro);
                return CreatedAtAction(nameof(ListarUsuarios), new { id = cadastro.Id }, cadastro);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar o usuário: {ex.Message}");
            }
        }

        //METODO PARA ALTERAR/ATUALIZAR/EDITAR INFORMAÇÕES DO USUARIO
        [HttpPut("AlterarUsuario/{id}")]
        public IActionResult AlterarUsuario(int id, [FromBody] Usuarios usuarioAtualizado)
        {
            if (usuarioAtualizado == null)
                return BadRequest("Dados do usuário inválidos.");

            try
            {
                var sucesso = _usuariosRepository.AlterarUsuario(id, usuarioAtualizado);
                if (!sucesso)
                    return NotFound("Usuário não encontrado.");

                return Ok("Usuário atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar o usuário: {ex.Message}");
            }
        }

        // METODO PARA LISTAR TODOS OS USUARIO INSERIDOS NO BANCO DE DADOS
        [HttpGet("ListarUsuarios")]
        public ActionResult<List<Usuarios>> ListarUsuarios()
        {
            try
            {
                var usuarios = _usuariosRepository.ListarUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter usuários: {ex.Message}");
            }
        }

        // METODO PARA DELETAR ALGUM USUARIO DO BANCO DE DADOS
        [HttpDelete("DeletarUsuario/{id}")]
        public IActionResult DeletarUsuario(int id)
        {
            try
            {
                var sucesso = _usuariosRepository.DeletarUsuario(id);
                if (!sucesso)
                    return NotFound("Usuário não encontrado.");

                return Ok("Usuário deletado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao deletar o usuário: {ex.Message}");
            }
        }

    }
}
