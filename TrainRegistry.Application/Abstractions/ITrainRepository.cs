using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Application.Abstractions
{
    public interface ITrainRepository
    {
        Task<IReadOnlyList<Train>> GetAllAsync(CancellationToken ct);
        Task<Train?> GetByIdAsync(Guid id, CancellationToken ct);

        Task<Guid> AddAsync(Train train, CancellationToken cancellationToken);

        Task<bool> UpdateAsync(Train train, CancellationToken cancellationToken);
    }
}
