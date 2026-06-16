using MediatR;
using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Application.Trains.Queries.GetAllTrains
{
    public record GetAllTrainsQuery : IRequest<IReadOnlyList<Train>>;
}
