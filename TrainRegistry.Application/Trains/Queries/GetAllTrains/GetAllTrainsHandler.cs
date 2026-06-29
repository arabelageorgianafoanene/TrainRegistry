using ErrorOr;
using MediatR;
using TrainRegistry.Application.Interfaces;
using TrainRegistry.Application.Trains.DTOs;

namespace TrainRegistry.Application.Trains.Queries.GetAllTrains
{
    public class GetAllTrainsHandler : IRequestHandler<GetAllTrainsQuery, ErrorOr<IReadOnlyList<TrainResponse>>>
    {
        private readonly ITrainRepository _repository;

        public GetAllTrainsHandler(ITrainRepository repository)
        {
            _repository = repository;
        }

        public async Task<ErrorOr<IReadOnlyList<TrainResponse>>> Handle(GetAllTrainsQuery request, CancellationToken ct)
        {
            var trains = await _repository.GetAllAsync(ct);

            if(trains.Any())
            {
                return trains.Select(train => train.ToTrainResponse()).ToList();
            }

            return new List<TrainResponse>();    
        }
    }
}
