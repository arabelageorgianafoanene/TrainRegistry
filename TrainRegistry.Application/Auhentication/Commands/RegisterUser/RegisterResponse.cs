
using TrainRegistry.Application.Common.Enums;

namespace TrainRegistry.Application.Auhentication.Commands.RegisterUser
{
    public record RegisterResponse(Guid Guid, string UserName, bool Success, string Message, RegisterUserErrorCode RegisterUserErrorCode);    
}
