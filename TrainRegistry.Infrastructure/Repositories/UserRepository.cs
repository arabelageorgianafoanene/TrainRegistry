using Microsoft.EntityFrameworkCore;
using TrainRegistry.Application.Interfaces;
using TrainRegistry.Domain.Entities;
using TrainRegistry.Infrastructure.Persistence;

namespace TrainRegistry.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TrainDbContext _trainDbContext;

        public UserRepository(TrainDbContext trainDbContext) 
        {
            _trainDbContext = trainDbContext;
        }

        public async Task<Guid> CreateUserAsync(User user, CancellationToken ct)
        {
            await _trainDbContext.Users.AddAsync(user, ct);
            await _trainDbContext.SaveChangesAsync(ct);

            return user.UserId;
        }

        public async Task<User?> GetPasswordHashAndSaltAsync(string name, CancellationToken ct) => await _trainDbContext.Users.FindAsync(name, ct);
           
        public async Task<bool> UserExistsAsync(string username, CancellationToken ct) => await _trainDbContext.Users.AnyAsync(u => u.Name == username, ct);
    }
}
