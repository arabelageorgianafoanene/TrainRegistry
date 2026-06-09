namespace TrainRegistry.Api.DTOs
{
    public record TrainDto(
     Guid Id,
     string Name,
     double Length,
     double Speed
 );
}
