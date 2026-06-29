using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Application.Trains.DTOs
{
    public static class TrainMapper
    {
        public static TrainResponse ToTrainResponse(this Train train)
        {
            return new TrainResponse(train.Id, train.Name, train.Length.Value, train.Speed.Value, train.TrainStatus.Value);
        }

    }
}
