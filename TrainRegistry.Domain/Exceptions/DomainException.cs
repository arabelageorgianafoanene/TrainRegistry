namespace TrainRegistry.src.TrainService.DomainModel.Exceptions
{
    public class DomainException: Exception
    {
        public DomainException() { }

        public DomainException(string message) : base(message) { }
    }
}
