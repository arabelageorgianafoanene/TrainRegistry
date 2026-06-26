namespace TrainRegistry.Domain.ValueObjects
{
    public sealed class TrainSpeed
    {
        public double Value { get; private set; }

        private TrainSpeed() { }

        public TrainSpeed(double speed)
        {
            if (speed < 0)
            {
                throw new ArgumentException("Train speed cannot be negative.");
            }
            
            Value = speed;
        }
    }
}
