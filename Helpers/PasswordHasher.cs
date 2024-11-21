using System.Security.Cryptography;

namespace Application.Helpers
{
    public class PasswordHasher
    {
        public static string HashPassword(string Senha)
        {
            // Gera um salt único para cada senha
            var salt = GenerateSalt();
            var pbkdf2 = new Rfc2898DeriveBytes(Senha, salt, 10000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32); // Tamanho do hash

            // Concatena o salt e o hash para salvar no banco
            var hashBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, hashBytes, 0, salt.Length);
            Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);

            // Converte para base64 para salvar no banco
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string Senha, string storedHash)
        {
            // Decodifica o hash salvo
            var hashBytes = Convert.FromBase64String(storedHash);

            // Extrai o salt
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Extrai o hash real
            var savedHash = new byte[32];
            Array.Copy(hashBytes, 16, savedHash, 0, 32);

            // Gera um hash da senha fornecida usando o mesmo salt
            var pbkdf2 = new Rfc2898DeriveBytes(Senha, salt, 10000, HashAlgorithmName.SHA256);
            var testHash = pbkdf2.GetBytes(32);

            // Compara os hashes
            return testHash.SequenceEqual(savedHash);
        }

        private static byte[] GenerateSalt()
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
