using Microsoft.Extensions.Logging;
using Moq;
using TrainRegistry.Application.Auhentication.Hashing;
using TrainRegistry.Application.Auhentication.Queries.LoginUser;
using TrainRegistry.Application.Common.Enums;
using TrainRegistry.Application.Interfaces;
using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Application.Tests.Authentication.Queries.LoginUser
{
    public class LoginUserHandlerTests
    {
        private readonly LoginUserHandler _handler;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;

        public LoginUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            var loggerMock = new Mock<ILogger<LoginUserHandler>>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

            _handler = new LoginUserHandler(_userRepositoryMock.Object, loggerMock.Object, _passwordHasherMock.Object, _jwtTokenGeneratorMock.Object);
        }

        [Fact]
        public async Task Should_Return_The_Token_When_User_Exists_And_The_Password_Matches()
        {
            var mockedToken = "mocked-jwt-token";

            User? user = new User
            {
                Id = Guid.NewGuid(),
                Name = "testuser",
            };

            _userRepositoryMock.Setup(x => x.UserExistsAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(true);
            _passwordHasherMock.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            _userRepositoryMock.Setup(x => x.GetPasswordHashAndSaltAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user); 
            _jwtTokenGeneratorMock.Setup(x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(mockedToken);

            var loginUserResponse = await _handler.Handle(new LoginUserQuery("testuser", "password123"), CancellationToken.None);

            Assert.True(loginUserResponse.Success);
            Assert.Equal(mockedToken, loginUserResponse.Token);
            Assert.Contains("Login successful", loginUserResponse.Message);
            Assert.Equal(LoginErrorCode.None, loginUserResponse.LoginErrorCode);

            _userRepositoryMock.Verify(x => x.UserExistsAsync("testuser", CancellationToken.None), Times.Once);
            _userRepositoryMock.Verify(x => x.GetPasswordHashAndSaltAsync("testuser", CancellationToken.None), Times.Once);
            _passwordHasherMock.Verify(x => x.VerifyPasswordHash("password123", It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            _jwtTokenGeneratorMock.Verify(x => x.GenerateToken(user.Id, "testuser"), Times.Once);
        }

        [Fact]
        public async Task Should_Not_Return_The_Token_When_User_Doesnot_Exists()
        {
            _userRepositoryMock.Setup(x => x.UserExistsAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(false);
            
            var loginUserResponse = await _handler.Handle(new LoginUserQuery("testuser", "password123"), CancellationToken.None);

            Assert.False(loginUserResponse.Success);
            Assert.Contains("Login unsuccessful", loginUserResponse.Message);
            Assert.Equal(LoginErrorCode.UserNotFound, loginUserResponse.LoginErrorCode);

            _userRepositoryMock.Verify(x => x.UserExistsAsync("testuser", CancellationToken.None), Times.Once);
            _userRepositoryMock.Verify(x => x.GetPasswordHashAndSaltAsync("testuser", CancellationToken.None), Times.Never);
            _passwordHasherMock.Verify(x => x.VerifyPasswordHash("password123", It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Never);
            _jwtTokenGeneratorMock.Verify(x => x.GenerateToken(It.IsAny<Guid>(), "testuser"), Times.Never);
        }

        [Fact]
        public async Task Should_Not_Return_The_Token_When_The_Password_Doesnot_Match()
        {
            var user= new User
            {
                Id = Guid.NewGuid(),
                Name = "testuser",
            };

            _userRepositoryMock.Setup(x => x.UserExistsAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(true);
            _userRepositoryMock.Setup(x => x.GetPasswordHashAndSaltAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _passwordHasherMock.Setup(x => x.VerifyPasswordHash("password123", It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);

            var loginUserResponse = await _handler.Handle(new LoginUserQuery("testuser", "password123"), CancellationToken.None);

            Assert.False(loginUserResponse.Success);
            Assert.Contains("Login unsuccessful", loginUserResponse.Message);
            Assert.Equal(LoginErrorCode.InvalidPassword, loginUserResponse.LoginErrorCode);

            _userRepositoryMock.Verify(x => x.UserExistsAsync("testuser", CancellationToken.None), Times.Once);
            _userRepositoryMock.Verify(x => x.GetPasswordHashAndSaltAsync("testuser", CancellationToken.None), Times.Once);
            _passwordHasherMock.Verify(x => x.VerifyPasswordHash("password123", It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            _jwtTokenGeneratorMock.Verify(x => x.GenerateToken(It.IsAny<Guid>(), "testuser"), Times.Never);
        }
    }
}
