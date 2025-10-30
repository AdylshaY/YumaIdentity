namespace YumaIdentity.Application.Interfaces
{
    using System.Collections.Generic;
    using YumaIdentity.Domain.Entities;

    public interface ITokenGenerator
    {
        string GenerateAccessToken(User user, Application application, IEnumerable<string> roles);
        RefreshToken GenerateRefreshToken(User user);
    }
}
