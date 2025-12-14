namespace YumaIdentity.Application.Features.OAuth.Queries.UserInfo
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Interfaces;

    /// <summary>
    /// Handles the UserInfo query by extracting claims from the current user.
    /// </summary>
    public class UserInfoQueryHandler : IRequestHandler<UserInfoQuery, UserInfoResponse>
    {
        private readonly ICurrentUserService _currentUserService;

        public UserInfoQueryHandler(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public Task<UserInfoResponse> Handle(UserInfoQuery request, CancellationToken cancellationToken)
        {
            var user = _currentUserService.User;

            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var email = user?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            var emailVerifiedClaim = user?.FindFirst("email_verified")?.Value;
            var roles = user?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList() ?? new();

            var response = new UserInfoResponse
            {
                Sub = userId,
                Email = email,
                EmailVerified = bool.TryParse(emailVerifiedClaim, out var verified) && verified,
                Roles = roles
            };

            return Task.FromResult(response);
        }
    }
}
