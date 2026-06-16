
using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Application.Interfaces
{
    public interface IUserRepository
    {
        public Task<Guid> CreateUserAsync(User user, CancellationToken ct);

        public Task<User?> GetPasswordHashAndSaltAsync(string username, CancellationToken ct);

        public Task<bool> UserExistsAsync(string name, CancellationToken ct);
    }
}
