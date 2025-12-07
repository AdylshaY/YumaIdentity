namespace YumaIdentity.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? ClientId { get; }
    }
}
