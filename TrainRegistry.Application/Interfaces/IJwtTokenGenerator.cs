namespace TrainRegistry.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(Guid userId, string userName);
    }
}
