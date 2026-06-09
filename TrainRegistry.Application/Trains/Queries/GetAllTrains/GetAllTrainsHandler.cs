using TrainRegistry.Domain;

namespace TrainRegistry.Application.Trains.Queries.GetAllTrains
{
    public class GetAllTrainsHandler
    {
        private readonly ITrainRepository _repository;

        public GetAllTrainsHandler(ITrainRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<Train>> Handle(GetAllTrainsQuery request, CancellationToken ct)
        {
            return await _repository.GetAllAsync(ct);
        }
    }
}
