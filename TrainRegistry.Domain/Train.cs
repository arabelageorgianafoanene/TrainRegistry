using TrainRegistry.src.TrainService.DomainModel.Exceptions;

namespace TrainRegistry.Domain
{
    public class Train
    {
        public  Guid Id { get; private set; }
        public  string Name { get; private set; }

        public double Length {  get; private set; }

        public  double Speed {  get; private set; }

        public Train(double Length, double Speed, string name)
        {
            Id = Guid.NewGuid();

            if (Speed < 0)
            {
                throw new DomainException("Train speed cannot be negative!");
            }

            if (Length < 0)
            {
                throw new DomainException("Train length cannot be negative!");
            }

            Name = name;
        }
    }
}
