namespace TrainRegistry.Application.Abstractions
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(Guid userId, string userName);
    }
}
