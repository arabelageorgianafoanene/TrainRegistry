using TrainRegistry.src.TrainService.DomainModel.Exceptions;

namespace TrainRegistry.Domain
{
    public class Train
    {
        public  Guid Id { get; private set; }
        public  string Name { get; private set; }

        public double Length {  get; private set; }

        public  double Speed {  get; private set; }

        public Train(double length, double speed, string name)
        {
            Id = Guid.NewGuid();

            if (speed < 0)
            {
                throw new DomainException("Train speed cannot be negative!");
            }

            if (length < 0)
            {
                throw new DomainException("Train length cannot be negative!");
            }

            Length = length;
            Speed = speed;
            Name = name;
        }
    }
}
