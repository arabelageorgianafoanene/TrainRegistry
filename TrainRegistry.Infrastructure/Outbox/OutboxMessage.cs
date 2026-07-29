
namespace TrainRegistry.Infrastructure.Outbox
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }              
        public required string Payload { get; set; }       
        public required bool Processed { get; set; }  
        public required DateTime UpdatedOnUtc { get; set; }

        public required string Type {  get; set; }               
    }
}
