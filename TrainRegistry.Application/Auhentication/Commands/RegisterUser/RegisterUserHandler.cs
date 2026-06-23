using MediatR;
using Microsoft.Extensions.Logging;
using TrainRegistry.Application.Auhentication.Hashing;
using TrainRegistry.Application.Common.Enums;
using TrainRegistry.Application.Interfaces;
using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Application.Auhentication.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(IUserRepository userRepository, ILogger<RegisterUserHandler> logger, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        async Task<RegisterResponse> IRequestHandler<RegisterUserCommand, RegisterResponse>.Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Registering user with name: {request.UserName}!");

            var userAlreadyExists = await _userRepository.UserExistsAsync(request.UserName, cancellationToken);

            if(userAlreadyExists)
            {
                _logger.LogInformation($"User name {request.UserName} already exists!");
                return new RegisterResponse(Guid.Empty, request.UserName, false, $"Registration unsuccessful: User name {request.UserName} already exists!", RegisterUserErrorCode.UserAlreadyExists);
            }

            _logger.LogInformation($"Hashing password for user: {request.UserName}!");
            var passwordHasherResult = _passwordHasher.CreatePasswordHash(request.Password);

            _logger.LogInformation($"Password hashed for user: {request.UserName}! ");

            var user = new User()
            {
                Name = request.UserName,
                PasswordSalt = Convert.ToBase64String(passwordHasherResult.PasswordSalt),
                PasswordHash = Convert.ToBase64String(passwordHasherResult.PasswordHash)
            };

            var guid = await _userRepository.CreateUserAsync(user, cancellationToken);

            if(guid == Guid.Empty)
            {
                _logger.LogWarning($"Failed to create user with name: {request.UserName}!");
                return new RegisterResponse(Guid.Empty, request.UserName, false, $"Registration unsuccessful: Failed to create user with name {request.UserName}!", RegisterUserErrorCode.DatabaseError);
            }

            _logger.LogInformation($"User name {request.UserName} registered successfully with id {user.Id}!");
            return new RegisterResponse(guid, request.UserName, true, $"User name {request.UserName} registered successfully with id {user.Id}!", RegisterUserErrorCode.None);
        }
    }
}
