namespace TrainRegistry.Domain.ValueObjects
{
    public sealed class TrainLength
    {
        public double Length { get; }

        public TrainLength(double length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Train length cannot be negative.");
            }

            Length = length;
        }
    }
}
