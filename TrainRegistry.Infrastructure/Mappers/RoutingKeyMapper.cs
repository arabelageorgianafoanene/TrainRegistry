using TrainEventContracts;

namespace TrainRegistry.Infrastructure.Mappers
{
    public static class RoutingKeyMapper
    {
        public static string Map(string eventType) => eventType switch
        {
            nameof(TrainRegistered) => TrainEventRoutingKeys.TrainRegistered,
            nameof(TrainStatusUpdated) => TrainEventRoutingKeys.TrainStatusChanged,
            _ => throw new NotImplementedException()
        }; 
    }
}
