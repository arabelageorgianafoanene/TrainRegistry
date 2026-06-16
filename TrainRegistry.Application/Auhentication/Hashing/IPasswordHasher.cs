namespace TrainRegistry.Application.Auhentication.Hashing
{
    public interface IPasswordHasher
    {
        public PasswordHashResult CreatePasswordHash(string password);

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    }
}
