
using TrainRegistry.Domain.Exceptions;

namespace TrainRegistry.Domain.ValueObjects
{
    public sealed class TrainStatus
    {
        public static readonly TrainStatus Active = new ("Active");
        public static readonly TrainStatus Inactive = new("Inactive");
        public static readonly TrainStatus UnderMaintenance = new("UnderMaintenance");
        public static readonly TrainStatus Decommissioned = new("Decommissioned");

        

        public string Value { get; }
        private TrainStatus(string value)
        {
            Value = value;
        }

        public static readonly IReadOnlyCollection<TrainStatus> All = new []
        {
            Active,
            Inactive,
            UnderMaintenance,
            Decommissioned
        };

        public static TrainStatus From(string value)
        {
            var status = All.FirstOrDefault(s => s.Value.Equals(value, StringComparison.OrdinalIgnoreCase));

            return status is null ? throw new InvalidTrainException(value) : status;
        }

        public bool CanTransitionTo(TrainStatus newStatus)
        {
            if(this == Inactive)
            {
                Console.WriteLine("Train is Inactive. It can only transition to Active or Decommissioned.");
            }

            if(newStatus == Active)
            {
                Console.WriteLine("Train is transitioning to Active. It can only transition from Inactive or UnderMaintenance.");
            }              


            if(this == newStatus) return true;

            return (this, newStatus) switch
            {
                _ when this == Active && newStatus == Inactive => true,
                _ when this == Active && newStatus == UnderMaintenance => true,
                _ when this == Inactive && newStatus == Active => true,
                _ when this == Inactive && newStatus == Decommissioned => true,
                _ when this == UnderMaintenance && newStatus == Active => true,
                _ when this == UnderMaintenance && newStatus == Decommissioned => true,
                _ => false
            };
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
        {
            if(obj is TrainStatus other)
            {
                return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public static bool operator ==(TrainStatus? left, TrainStatus? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(TrainStatus? left, TrainStatus? right) => !(left == right);

        public override int GetHashCode() => Value.GetHashCode();
    }
}
