using System.Security.Cryptography;
using TrainRegistry.Application.Auhentication.Hashing;

namespace TrainRegistry.Infrastructure.Authentication.Hashing
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16; 
        private const int HashSize = 32;
        private const int Iterations = 100000;

        public PasswordHashResult CreatePasswordHash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);

            using Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(HashSize);
            return new PasswordHashResult
            {
                PasswordHash = hash,
                PasswordSalt = salt
            };
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, Iterations, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(HashSize);
            var result = new PasswordHashResult
            {
                PasswordHash = hash,
                PasswordSalt = storedSalt
            };

            return result.PasswordHash.SequenceEqual(storedHash);
        }
    }
}
