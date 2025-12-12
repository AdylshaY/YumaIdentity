namespace YumaIdentity.Application.Features.OAuth.Queries.UserInfo
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    /// <summary>
    /// Query to get the current authenticated user's information.
    /// </summary>
    public class UserInfoQuery : IRequest<UserInfoResponse>
    {
    }
}
