namespace YumaIdentity.Application.Features.OAuth.Commands.RevokeToken
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Interfaces;

    /// <summary>
    /// Handles token revocation requests.
    /// Revokes refresh tokens to invalidate user sessions.
    /// </summary>
    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenRequest, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IClientValidator _clientValidator;
        private readonly ILogger<RevokeTokenCommandHandler> _logger;

        public RevokeTokenCommandHandler(
            IAppDbContext context,
            IClientValidator clientValidator,
            ILogger<RevokeTokenCommandHandler> logger)
        {
            _context = context;
            _clientValidator = clientValidator;
            _logger = logger;
        }

        public async Task<Unit> Handle(RevokeTokenRequest request, CancellationToken cancellationToken)
        {
            // Validate client exists
            var application = await _clientValidator.GetApplicationByClientIdAsync(request.ClientId);
            if (application == null)
            {
                // Per RFC 7009, return success even if client is invalid (don't leak info)
                _logger.LogWarning("Revoke request for unknown client: {ClientId}", request.ClientId);
                return Unit.Value;
            }

            // Hash the token to find it in database
            var tokenHash = HashToken(request.Token);

            // Try to find and revoke the refresh token
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => 
                    rt.TokenHash == tokenHash && 
                    rt.ApplicationId == application.Id && 
                    !rt.IsRevoked, 
                    cancellationToken);

            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                refreshToken.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Refresh token revoked for user {UserId} on application {AppName}",
                    refreshToken.UserId,
                    application.AppName);
            }
            else
            {
                // Per RFC 7009, return success even if token not found
                _logger.LogDebug("Token not found or already revoked");
            }

            // Note: Access tokens are stateless JWTs and cannot be revoked.
            // They will expire naturally. For immediate invalidation,
            // implement a token blacklist or short expiration times.

            return Unit.Value;
        }

        private static string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }
    }
}
