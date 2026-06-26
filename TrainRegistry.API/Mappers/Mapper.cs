using TrainRegistry.API.DTOs.Requests;
using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Api.Mappers
{
    public static class Mapper
    {
        public static CreateTrainRequest ToDTO(Train train)
        {          
            return new CreateTrainRequest(train.Id, train.Name, train.Length.Value, train.Speed.Value);
        }
    }
}
