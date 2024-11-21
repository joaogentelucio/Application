using Application.Helpers;
using Application.Models;
using FirebirdSql.Data.FirebirdClient;

namespace Application.Repositories
{
    public class UsuariosRepository : IUsuarioRepository
    {
        private readonly string _connectionString;
        private string? connectionString;

        public UsuariosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("FirebirdConnection");
        }

        public UsuariosRepository(string? connectionString)
        {
            this.connectionString = connectionString;
        }

        // IMPLEMENTÇÃO DE LOGICA PARA EFETUAR OS CRUDs

        // LOGICA PARA INSERIR/SALVAR USUARIO NO BANCO DE DADOS 
        public void Salvar(Usuarios usuarios)
        {
            using (var connection = new FbConnection(_connectionString))
            {
                connection.Open();

                // Hash da senha antes de salvar no banco
                var hashedPassword = PasswordHasher.HashPassword(usuarios.Senha);
                var query = "INSERT INTO usuarios (nome, email, senha) VALUES (@Nome, @Email, @Senha)";
                using (var command = new FbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nome", usuarios.Nome);
                    command.Parameters.AddWithValue("@Email", usuarios.Email);
                    command.Parameters.AddWithValue("@Senha", hashedPassword);

                    // Recupera o ID gerado automaticamente
                    var newId = command.ExecuteScalar(); // Recupera o valor do ID gerado
                    usuarios.Id = Convert.ToInt32(newId); // Atribui o ID ao objeto
                }
            }
        }

        // LOGICA PARA EDITAR/ALTERAR/ATUALIZAR USUARIO NO BANCO DE DADOS
        public bool AlterarUsuario(int id, Usuarios usuarioAtualizado)
        {
            using (var connection = new FbConnection(_connectionString))
            {
                connection.Open();

                // Criptografa a senha apenas se ela não estiver vazia
                var hashedPassword = string.IsNullOrEmpty(usuarioAtualizado.Senha) ? null : PasswordHasher.HashPassword(usuarioAtualizado.Senha);

                // Construção da query
                var query = "UPDATE usuarios SET ";

                // Lista para armazenar as condições
                var setClauses = new List<string>();

                // Verifica se o nome foi fornecido
                if (!string.IsNullOrEmpty(usuarioAtualizado.Nome))
                {
                    setClauses.Add("nome = @Nome");
                }

                // Verifica se o email foi fornecido
                if (!string.IsNullOrEmpty(usuarioAtualizado.Email))
                {
                    setClauses.Add("email = @Email");
                }

                // Verifica se a senha foi fornecida
                if (hashedPassword != null)
                {
                    setClauses.Add("senha = @Senha");
                }

                // Se nenhum campo foi alterado, retorna falso
                if (setClauses.Count == 0)
                {
                    return false;
                }

                // Junta as partes da query
                query += string.Join(", ", setClauses) + " WHERE id = @Id";

                // Executa o comando de atualização
                using (var command = new FbCommand(query, connection))
                {
                    // Adiciona os parâmetros
                    if (!string.IsNullOrEmpty(usuarioAtualizado.Nome))
                        command.Parameters.AddWithValue("@Nome", usuarioAtualizado.Nome);

                    if (!string.IsNullOrEmpty(usuarioAtualizado.Email))
                        command.Parameters.AddWithValue("@Email", usuarioAtualizado.Email);

                    if (hashedPassword != null)
                        command.Parameters.AddWithValue("@Senha", hashedPassword);

                    command.Parameters.AddWithValue("@Id", id);

                    var rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }


        // LOGICA PARA LISTAR TODOS OS USUARIOS INSERIDOS NO BANCO DE DADOS
        public List<Usuarios> ListarUsuarios()
        {
            var usuariosList = new List<Usuarios>();

            using (var connection = new FbConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT id, nome, email, senha FROM usuarios";
                using (var command = new FbCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuariosList.Add(new Usuarios
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Email = reader.GetString(2),
                            Senha = reader.GetString(3)
                        });
                    }
                }
            }

            return usuariosList;
        }

        // LOGICA PARA DELETAR ALGUM USUARIO NO BANCO DE DADOS
        public bool DeletarUsuario(int id)
        {
            using (var connection = new FbConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM usuarios WHERE id = @Id";
                using (var command = new FbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // LOGICA PARA VERIFICAR SE EMAIL E VALIDO 
        public Usuarios GetUsuariosByEmail(string email)
        {
            using (var connection = new FbConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT id, nome, email, senha FROM usuarios WHERE email = @Email";
                using (var command = new FbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuarios
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Nome = reader.GetString(reader.GetOrdinal("nome")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                Senha = reader.GetString(reader.GetOrdinal("senha"))
                            };
                        }
                    }
                }
            }

            return null;
        }

    }
}
