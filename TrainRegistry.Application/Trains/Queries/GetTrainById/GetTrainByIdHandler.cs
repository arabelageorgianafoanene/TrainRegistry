using ErrorOr;
using MediatR;
using TrainRegistry.Application.Interfaces;
using TrainRegistry.Application.Trains.DTOs;

namespace TrainRegistry.Application.Trains.Queries.GetTrainById
{
    public class GetTrainByIdHandler: IRequestHandler<GetTrainByIdQuery, ErrorOr<TrainResponse>>
    {
        private readonly ITrainRepository _repository;

        public GetTrainByIdHandler(ITrainRepository repository) => _repository = repository;

        public async Task<ErrorOr<TrainResponse>> Handle(GetTrainByIdQuery request, CancellationToken ct)
        {
            var train = await _repository.GetByIdAsync(request.Id, ct);

            if(train is null)
            {
                return Error.NotFound("Train.NotFound", "No train was found!");
            }

            return train.ToTrainResponse();
        }
    }
}
