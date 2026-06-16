namespace TrainRegistry.API.DTOs.Requests
{
    public record CreateTrainRequest(
     Guid Id,
     string Name,
     double Length,
     double Speed
 );
}
