using MediatR;
using Microsoft.Extensions.Logging;
using TrainRegistry.Application.Auhentication.Hashing;
using TrainRegistry.Application.Common.Enums;
using TrainRegistry.Application.Interfaces;

namespace TrainRegistry.Application.Auhentication.Queries.LoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserQuery, LoginResponse>
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<LoginUserHandler> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserHandler(IUserRepository repository, ILogger<LoginUserHandler> logger, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _repository = repository;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponse> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login attempt for user name {Name}", request.UserName);

            var userExists = await _repository.UserExistsAsync(request.UserName, cancellationToken);

            if (!userExists)
            {
                _logger.LogWarning("User name {Name} does not exist", request.UserName);
                return new LoginResponse(request.UserName, string.Empty, false, $"Login unsuccessful: User name {request.UserName} does not exist", LoginErrorCode.UserNotFound);
            }

            var user = await _repository.GetPasswordHashAndSaltAsync(request.UserName, cancellationToken);
            if (user != null)
            {
                var passwordHash = Convert.FromBase64String(user.PasswordHash);
                var passwordSalt = Convert.FromBase64String(user.PasswordSalt);

                var isPasswordValid = _passwordHasher.VerifyPasswordHash(request.Password, passwordHash, passwordSalt);

                if (isPasswordValid)
                {
                    _logger.LogInformation("User name {Name} logged in successfully", request.UserName);
                    var token = _jwtTokenGenerator.GenerateToken(user.UserId, request.UserName);
                    return new LoginResponse(request.UserName, token, true, "Login successful", LoginErrorCode.None);
                }

                _logger.LogWarning("Invalid password for user name {Name}", request.UserName);
                return new LoginResponse(request.UserName, string.Empty, false, $"Login unsuccessful: Invalid password for user name {request.UserName}", LoginErrorCode.InvalidPassword);
            }

            _logger.LogWarning("User name {Name} not found", request.UserName);
            return new LoginResponse(request.UserName, string.Empty, false, $"Login unsuccessful: User name {request.UserName} not found!", LoginErrorCode.UserNotFound);
        }
    }
}
