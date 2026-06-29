using ErrorOr;
using MediatR;
using TrainRegistry.Application.Trains.DTOs;

namespace TrainRegistry.Application.Trains.Queries.GetTrainById
{
    public record GetTrainByIdQuery(Guid Id) : IRequest<ErrorOr<TrainResponse>>;
    
}
