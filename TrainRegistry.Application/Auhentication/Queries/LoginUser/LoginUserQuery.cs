using MediatR;

namespace TrainRegistry.Application.Auhentication.Queries.LoginUser
{
    public record LoginUserQuery(string UserName, string Password) : IRequest<LoginResponse>;
}
