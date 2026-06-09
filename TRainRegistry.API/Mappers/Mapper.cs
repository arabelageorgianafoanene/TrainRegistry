using TrainRegistry.Api.DTOs;
using TrainRegistry.Domain;

namespace TrainRegistry.Api.Mappers
{
    public static class Mapper
    {
        public static TrainDto ToDTO(Train train)
        {
            return new TrainDto(train.Id, train.Name, train.Length, train.Speed);
        }
    }
}
