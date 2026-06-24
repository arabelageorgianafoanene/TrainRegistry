using MediatR;
using Moq;
using TrainRegistry.Application.Auhentication.Commands.RegisterUser;
using TrainRegistry.Application.Auhentication.Hashing;
using TrainRegistry.Application.Common.Enums;
using TrainRegistry.Application.Interfaces;

namespace TrainRegistry.Application.Tests.Authentication.Commands.RegisterUser
{
    public class RegisterUserHandlerTests
    {
        private readonly IRequestHandler<RegisterUserCommand, RegisterResponse> _registerUserHandler;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<Microsoft.Extensions.Logging.ILogger<RegisterUserHandler>> _loggerMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;

        public RegisterUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<RegisterUserHandler>>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _registerUserHandler = new RegisterUserHandler(_userRepositoryMock.Object, _loggerMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_RegisterUserResponse_When_User_Is_Registered_Successfully()
        {
            _userRepositoryMock.Setup(repo => repo.UserExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _passwordHasherMock.Setup(hasher => hasher.CreatePasswordHash(It.IsAny<string>())).Returns(new PasswordHashResult() { PasswordHash = new byte[] { 1, 2, 3 }, PasswordSalt = new byte[] { 4, 5, 6 } });

            var guid = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<Domain.Entities.User>(), It.IsAny<CancellationToken>())).ReturnsAsync(guid);

            var registerUserResponse = await _registerUserHandler.Handle(new RegisterUserCommand("testuser", "password123"), CancellationToken.None);

            Assert.Contains("registered successfully", registerUserResponse.Message);
            Assert.Equal(guid, registerUserResponse.Guid);
            Assert.Equal(RegisterUserErrorCode.None, registerUserResponse.RegisterUserErrorCode);

            _userRepositoryMock.Verify(repo => repo.UserExistsAsync("testuser", It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.CreateUserAsync(It.IsAny<Domain.Entities.User>(), It.IsAny<CancellationToken>()), Times.Once);
            _passwordHasherMock.Verify(hasher => hasher.CreatePasswordHash("password123"), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Not_Return_Successfully_When_User_Already_Exists() 
        {
            _userRepositoryMock.Setup(repo => repo.UserExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
                      
            var registerUserResponse = await _registerUserHandler.Handle(new RegisterUserCommand("testuser", "password123"), CancellationToken.None);

            Assert.False(registerUserResponse.Success);
            Assert.Contains("already exists", registerUserResponse.Message);
            Assert.Equal(Guid.Empty, registerUserResponse.Guid);
            Assert.Equal(RegisterUserErrorCode.UserAlreadyExists, registerUserResponse.RegisterUserErrorCode);

            _userRepositoryMock.Verify(repo => repo.UserExistsAsync("testuser", It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.CreateUserAsync(It.IsAny<Domain.Entities.User>(), It.IsAny<CancellationToken>()), Times.Never);
            _passwordHasherMock.Verify(hasher => hasher.CreatePasswordHash("password123"), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Not_Return_Successfully_When_User_Creation_Fails()
        {
            _userRepositoryMock.Setup(repo => repo.UserExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _passwordHasherMock.Setup(hasher => hasher.CreatePasswordHash(It.IsAny<string>())).Returns(new PasswordHashResult() { PasswordHash = new byte[] { 1, 2, 3 }, PasswordSalt = new byte[] { 4, 5, 6 } });

            _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<Domain.Entities.User>(), It.IsAny<CancellationToken>())).ReturnsAsync(Guid.Empty);

            var registerUserResponse = await _registerUserHandler.Handle(new RegisterUserCommand("testuser", "password123"), CancellationToken.None);

            Assert.False(registerUserResponse.Success);

            Assert.Contains("Registration unsuccessful", registerUserResponse.Message);

            Assert.Equal(Guid.Empty, registerUserResponse.Guid);
            Assert.Equal(RegisterUserErrorCode.DatabaseError, registerUserResponse.RegisterUserErrorCode);

            _userRepositoryMock.Verify(repo => repo.UserExistsAsync("testuser", It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.CreateUserAsync(It.IsAny<Domain.Entities.User>(), It.IsAny<CancellationToken>()), Times.Once);
            _passwordHasherMock.Verify(hasher => hasher.CreatePasswordHash("password123"), Times.Once);
        }
    }
}
