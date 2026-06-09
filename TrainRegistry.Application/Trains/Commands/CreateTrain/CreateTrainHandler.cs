using TrainRegistry.Domain;

namespace TrainRegistry.Application.Trains.Commands.CreateTrain
{
    public class CreateTrainHandler
    {
        private readonly ITrainRepository _repository;

        public CreateTrainHandler(ITrainRepository trainRepository)
        {
            _repository = trainRepository;
        }

        public async Task<Guid> Handle(CreateTrainCommand command, CancellationToken cancellationToken)
        {
            var train = new Train(command.Length, command.Speed, command.Name);
            
            await _repository.AddAsync(train, cancellationToken);

            return train.Id;
        }
    }
}
