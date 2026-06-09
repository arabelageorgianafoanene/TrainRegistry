using TrainRegistry.src.TrainService.DomainModel.Exceptions;

namespace TrainRegistry.src.TrainService.DomainModel.ValueObjects
{
    public class BrakingProfile
    {
        public required double ServiceBrakeDecelaration {  get; set; }

        public required double EmergencyBrakeDeceleration { get; set; }

        public required double ReactionTime {  get; set; }

        public required double BrakeBuildUpTime{  get; set; }

        public required double TrainMass {  get; set; }


        public required BrakeType BrakeType { get; set; }

        public required double SafetyMargin {  get; set; }


        public BrakingProfile(double serviceBrakeDecelaration, double emergencyBrakeDeceleration, double reactionTime,
            double brakeBuildUpTime, double trainMass, BrakeType brakeType, double safetyMargin)
        {
            if (serviceBrakeDecelaration < 0)
            {
                throw new DomainException("Service brake decelaration cannot be less then 0!");
            }

            if (emergencyBrakeDeceleration < 0)
            {
                throw new DomainException("Emergency brake decelaration cannot be less then 0!");
            }

            if (brakeBuildUpTime < 0)
            {
                throw new DomainException("Brake build up time cannot be less then 0!");
            }

            if (trainMass < 0)
            {
                throw new DomainException("Train mass cannot be less then 0!");
            }

            if (safetyMargin < 0)
            {
                throw new DomainException("afety margin cannot be less then 0!");
            }

            ServiceBrakeDecelaration = serviceBrakeDecelaration;
            EmergencyBrakeDeceleration = emergencyBrakeDeceleration;
            BrakeBuildUpTime = brakeBuildUpTime;
            TrainMass = trainMass;
            SafetyMargin = safetyMargin;
            BrakeType = brakeType;
            ReactionTime = reactionTime;
        }

    }
}
