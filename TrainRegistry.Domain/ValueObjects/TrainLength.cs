namespace TrainRegistry.Domain.ValueObjects
{
    public sealed class TrainLength
    {
        public double Value { get; private set; }

        private TrainLength() { }

        public TrainLength(double value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Train length cannot be negative.");
            }

            Value = value;
        }
    }
}
