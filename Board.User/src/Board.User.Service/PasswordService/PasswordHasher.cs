using System.Security.Cryptography;
using Board.User.Services.PasswordService.Interfaces;
using Board.User.Services.Settings;
using Microsoft.Extensions.Options;

namespace Board.User.Services.Password
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly IOptions<PasswordHasherSettings> _options;

        public PasswordHasher(IOptions<PasswordHasherSettings> options)
        {
            _options = options;
        }

        public string HashPassword(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(
                password,
                _options.Value.SaltSize,
                _options.Value.Iterations,
                HashAlgorithmName.SHA512
            );

            string key = Convert.ToBase64String(algorithm.GetBytes(_options.Value.KeySize));
            string salt = Convert.ToBase64String(algorithm.Salt);

            return $"{_options.Value.Iterations}.{salt}.{key}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            string[] parts = hashedPassword.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format.");
            }

            int iterations = Convert.ToInt32(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] key = Convert.FromBase64String(parts[2]);

            using var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA512
            );

            byte[] keyToCheck = algorithm.GetBytes(_options.Value.KeySize);

            return keyToCheck.SequenceEqual(key);
        }
    }
}
