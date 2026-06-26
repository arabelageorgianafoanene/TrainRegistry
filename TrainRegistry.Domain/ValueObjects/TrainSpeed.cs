namespace TrainRegistry.Domain.ValueObjects
{
    public sealed class TrainSpeed
    {
        public double Speed { get; }

        public TrainSpeed(double speed)
        {
            if (speed < 0)
            {
                throw new ArgumentException("Train speed cannot be negative.");
            }
            Speed = speed;
        }

    }
}
