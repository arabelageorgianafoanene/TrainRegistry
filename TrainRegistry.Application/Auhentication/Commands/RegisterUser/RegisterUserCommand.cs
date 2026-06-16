using MediatR;

namespace TrainRegistry.Application.Auhentication.Commands.RegisterUser
{
    public record RegisterUserCommand(string UserName, string Password) : IRequest<RegisterResponse>;
}
