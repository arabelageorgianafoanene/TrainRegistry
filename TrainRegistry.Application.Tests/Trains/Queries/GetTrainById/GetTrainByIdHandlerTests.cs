using Moq;
using TrainRegistry.Application.Interfaces;
using TrainRegistry.Application.Trains.Queries.GetTrainById;
using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Application.Tests.Trains.Queries.GetTrainById
{
    public  class GetTrainByIdHandlerTests
    {
        private readonly GetTrainByIdHandler _handler;
        private readonly Mock<ITrainRepository> _repositoryMock;

        public GetTrainByIdHandlerTests()
        {
            _repositoryMock = new Mock<ITrainRepository>();
            _handler = new GetTrainByIdHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Should_Return_Train_When_Train_Exists()
        {
            var trainId = Guid.NewGuid();
            var expectedTrain = Train.Create("test train", 10, 100);
                        
            _repositoryMock.Setup(repo => repo.GetByIdAsync(trainId, CancellationToken.None)).ReturnsAsync(expectedTrain);

            var handler = new GetTrainByIdHandler(_repositoryMock.Object);
            
            var result = await handler.Handle(new GetTrainByIdQuery(trainId), CancellationToken.None);
            
            Assert.NotNull(result);
            Assert.Equal(expectedTrain.Id, result.Id);
            Assert.Equal(expectedTrain.Name, result.Name);
            Assert.Equal(expectedTrain.Length, result.Length);
            Assert.Equal(expectedTrain.Speed, result.Speed);
        }

        [Fact]
        public async Task Should_No_Return_Any_Train_When_Train_Doesnot_Exists()
        {
            Train? train = null;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(train);

            var result = await _handler.Handle(new GetTrainByIdQuery(It.IsAny<Guid>()), CancellationToken.None);

            Assert.Null(result);           
        }
    }
}
