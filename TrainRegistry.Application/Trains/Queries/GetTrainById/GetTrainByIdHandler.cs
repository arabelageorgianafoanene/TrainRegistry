using MediatR;
using TrainRegistry.Application.Interfaces;
using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Application.Trains.Queries.GetTrainById
{
    public class GetTrainByIdHandler: IRequestHandler<GetTrainByIdQuery, Train?>
    {
        private readonly ITrainRepository _repository;

        public GetTrainByIdHandler(ITrainRepository repository) => _repository = repository;

        public async Task<Train?> Handle(GetTrainByIdQuery request, CancellationToken ct)
        {
            return await _repository.GetByIdAsync(request.Id, ct);
        }
    }
}
