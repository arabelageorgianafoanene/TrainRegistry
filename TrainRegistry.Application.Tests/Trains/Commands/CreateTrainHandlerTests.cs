using Microsoft.Extensions.Logging;
using Moq;
using TrainRegistry.Application.Interfaces;
using TrainRegistry.Application.Trains.Commands.CreateTrain;

namespace TrainRegistry.Application.Tests.Trains.Commands
{
    public class CreateTrainHandlerTests
    {
        private readonly CreateTrainHandler _createTrainHandler;
        private readonly Mock<ITrainRepository> _trainRepositoryMock;
        private readonly Mock<ILogger<CreateTrainHandler>> _loggerMock;

        public CreateTrainHandlerTests()
        {
            _trainRepositoryMock = new Mock<ITrainRepository>();
            _loggerMock = new Mock<ILogger<CreateTrainHandler>>();
            _createTrainHandler = new CreateTrainHandler(_trainRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Should_Create_Train_Succefully_When_Train_Added_Successfully()
        {
            var expectedGuid = Guid.NewGuid();
            _trainRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.Train>(), CancellationToken.None)).ReturnsAsync(expectedGuid);

            var actualGuid =  await _createTrainHandler.Handle(new CreateTrainCommand("Test Train", 20, 30), CancellationToken.None);

            Assert.Equal(expectedGuid, actualGuid);
        }

        [Fact]
        public async Task Should_Not_Create_Train_Succefully_When_Train_Not_Added_Successfully()
        {
            _trainRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.Train>(), CancellationToken.None)).ReturnsAsync(Guid.Empty);

            var actualGuid = await _createTrainHandler.Handle(new CreateTrainCommand("Test Train", 20, 30), CancellationToken.None);

            Assert.Equal(Guid.Empty, actualGuid);
        }
    }
}
