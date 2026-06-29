using Microsoft.EntityFrameworkCore;
using TrainRegistry.Infrastructure.Persistence;
using TrainRegistry.Application.Interfaces;
using TrainRegistry.Domain.Entities;
using TrainRegistry.Domain.ValueObjects;

namespace TrainRegistry.Infrastructure.Repositories
{
    public class TrainRepository : ITrainRepository
    {
        private readonly TrainDbContext _context;

        public TrainRepository(TrainDbContext trainDbContext)
        {
            _context = trainDbContext;
        }

        public async Task<IReadOnlyList<Train>> GetAllAsync(CancellationToken ct) => await _context.Trains.ToListAsync(ct);
        
        public async Task<Train?> GetByIdAsync(Guid id, CancellationToken ct) => await _context.Trains.FindAsync(id, ct);

        public async Task<Guid> AddAsync(Train train, CancellationToken cancellationToken)
        {
            await _context.Trains.AddAsync(train, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return train.Id;
        }

        public async Task<bool> UpdateAsync(Train train, CancellationToken cancellationToken)
        {
            
            _context.Trains.Update(train);
            await _context.SaveChangesAsync(cancellationToken);
            
            return true;
        }
    }
}
