using MediatR;
using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Application.Trains.Queries.GetTrainById
{
    public record GetTrainByIdQuery(Guid Id) : IRequest<Train?>;
    
}
