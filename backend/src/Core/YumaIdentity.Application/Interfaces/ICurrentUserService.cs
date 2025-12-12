namespace YumaIdentity.Application.Interfaces
{
    using System.Security.Claims;

    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? ClientId { get; }
        ClaimsPrincipal? User { get; }
    }
}

