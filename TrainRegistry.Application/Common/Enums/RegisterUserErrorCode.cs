namespace TrainRegistry.Application.Common.Enums
{
    public enum RegisterUserErrorCode
    {
        None = 0,
        UserAlreadyExists = 1,
        InvalidUsername = 2,
        InvalidPassword = 3,
        WeakPassword = 4,
        ValidationFailed = 6,
        DatabaseError = 7
    }
}
