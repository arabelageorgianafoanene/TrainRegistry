using TrainRegistry.Domain.ValueObjects;

namespace TrainRegistry.API.DTOs.Requests
{
    public record UpdateTrainStatusRequest (Guid TrainId, TrainStatus TrainStatus);
}
