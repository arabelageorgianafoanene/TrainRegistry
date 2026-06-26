namespace TrainRegistry.Domain.Exceptions
{
    public class InvalidTrainException: Exception
    {
        public InvalidTrainException(string value) : base($"{value} is not a valid train status!") { }
    }
}
