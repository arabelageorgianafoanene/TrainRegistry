using ErrorOr;
using MediatR;
using TrainRegistry.Application.Trains.DTOs;

namespace TrainRegistry.Application.Trains.Queries.GetAllTrains
{
    public record GetAllTrainsQuery : IRequest<ErrorOr<IReadOnlyList<TrainResponse>>>;
}
