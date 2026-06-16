using TrainRegistry.Application.Common.Enums;

namespace TrainRegistry.Application.Auhentication.Queries.LoginUser
{
    public record LoginResponse(string UserName, string Token, bool Success, string Message, LoginErrorCode LoginErrorCode);
}
