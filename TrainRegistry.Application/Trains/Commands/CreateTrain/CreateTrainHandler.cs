using Microsoft.Extensions.Logging;
using TrainRegistry.Domain;

namespace TrainRegistry.Application.Trains.Commands.CreateTrain
{
    public class CreateTrainHandler
    {
        private readonly ITrainRepository _repository;
        private readonly ILogger<CreateTrainHandler> _logger;

        public CreateTrainHandler(ITrainRepository trainRepository, ILogger<CreateTrainHandler> logger)
        {
            _repository = trainRepository;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateTrainCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Creating train with length: {command.Length}, speed: {command.Speed} and name: {command.Name}!");

            var train = new Train(command.Length, command.Speed, command.Name);
            
            await _repository.AddAsync(train, cancellationToken);

            _logger.LogInformation($"Train {train.Id} successfully created! ");

            return train.Id;
        }
    }
}
