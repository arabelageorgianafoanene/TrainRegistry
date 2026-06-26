
namespace TrainRegistry.Domain.Exceptions
{
    public class InvalidTrainStatusChangeException: Exception
    {
        public InvalidTrainStatusChangeException(string oldStatus, string newStatus): base($"The train cannot transit from {oldStatus} to the {newStatus}!") { }
    }
}
