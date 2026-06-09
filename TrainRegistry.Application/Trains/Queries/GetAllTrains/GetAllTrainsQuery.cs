using MediatR;
using TrainRegistry.Domain;

namespace TrainRegistry.Application.Trains.Queries.GetAllTrains
{
    public record GetAllTrainsQuery : IRequest<IReadOnlyList<Train>>;
}
