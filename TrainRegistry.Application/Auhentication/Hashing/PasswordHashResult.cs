
namespace TrainRegistry.Application.Auhentication.Hashing
{
    public class PasswordHashResult
    {
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt {  get; set; }
    }
}
