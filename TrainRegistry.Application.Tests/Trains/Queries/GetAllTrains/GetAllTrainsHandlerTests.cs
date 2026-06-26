
using Moq;
using TrainRegistry.Application.Interfaces;
using TrainRegistry.Application.Trains.Queries.GetAllTrains;
using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Application.Tests.Trains.Queries.GetAllTrains
{
    public class GetAllTrainsHandlerTests
    {
        private readonly GetAllTrainsHandler _handler;
        private readonly Mock<ITrainRepository> _repositoryMock;

        public GetAllTrainsHandlerTests()
        {
            _repositoryMock = new Mock<ITrainRepository>();
            _handler = new GetAllTrainsHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Should_Return_All_Trains_When_Trains_Are_Available()
        {
            _repositoryMock.Setup(repo => repo.GetAllAsync(CancellationToken.None)).ReturnsAsync(new List<Train>
            {
                Train.Create("test train 1", 10, 100),
                Train.Create("test train 2", 20, 200)
            });

            var list = await _handler.Handle(new GetAllTrainsQuery(), CancellationToken.None);

           Assert.NotNull(list);
           Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task Should_Not_Return_Anything_Trains_When_Trains_Are_Not_Available()
        {
            IReadOnlyList<Train> trains;

            _repositoryMock.Setup(repo => repo.GetAllAsync(CancellationToken.None)).ReturnsAsync(trains = new List<Train>());

            var list = await _handler.Handle(new GetAllTrainsQuery(), CancellationToken.None);

            Assert.NotNull(list);
            Assert.Empty(list);
        }
    }
}
