namespace TrainRegistry.Application.Trains.DTOs
{
    public record TrainResponse(Guid Id, string Name, double Length, double Speed, string Status);    
}
