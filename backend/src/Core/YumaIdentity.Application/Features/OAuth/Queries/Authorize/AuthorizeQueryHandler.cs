namespace YumaIdentity.Application.Features.OAuth.Queries.Authorize
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Domain.Entities;
    using YumaIdentity.Domain.Enums;

    /// <summary>
    /// Handles OAuth2 authorization request.
    /// Checks authentication status and generates authorization code if authenticated.
    /// </summary>
    public class AuthorizeQueryHandler : IRequestHandler<AuthorizeQuery, AuthorizeQueryResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IOAuthSessionService _sessionService;
        private readonly IPkceService _pkceService;
        private readonly ILogger<AuthorizeQueryHandler> _logger;

        private const int AuthorizationCodeExpirationSeconds = 60;

        public AuthorizeQueryHandler(
            IAppDbContext context,
            IOAuthSessionService sessionService,
            IPkceService pkceService,
            ILogger<AuthorizeQueryHandler> logger)
        {
            _context = context;
            _sessionService = sessionService;
            _pkceService = pkceService;
            _logger = logger;
        }

        public async Task<AuthorizeQueryResponse> Handle(AuthorizeQuery request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(a => a.ClientId == request.ClientId, cancellationToken);

            if (application == null)
            {
                _logger.LogWarning("Authorization failed: Unknown client_id {ClientId}", request.ClientId);
                throw new NotFoundException("Application", request.ClientId);
            }

            if (application.ClientType == ClientType.Public)
            {
                if (string.IsNullOrEmpty(request.CodeChallenge))
                {
                    throw new ValidationException("PKCE code_challenge is required for public clients.");
                }

                if (request.CodeChallengeMethod != "S256")
                {
                    throw new ValidationException("Only S256 code_challenge_method is supported.");
                }
            }

            if (!IsValidRedirectUri(application, request.RedirectUri))
            {
                _logger.LogWarning("Authorization failed: Invalid redirect_uri {RedirectUri} for client {ClientId}",
                    request.RedirectUri, request.ClientId);
                throw new ValidationException("Invalid redirect_uri.");
            }

            if (string.IsNullOrEmpty(request.SessionId))
            {
                return new AuthorizeQueryResponse
                {
                    RequiresAuthentication = true,
                    RequiresConsent = false
                };
            }

            var userId = _sessionService.GetUserIdFromSession(request.SessionId);
            if (!userId.HasValue)
            {
                return new AuthorizeQueryResponse
                {
                    RequiresAuthentication = true,
                    RequiresConsent = false
                };
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId.Value, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Authorization failed: User not found for session");
                throw new ValidationException("Invalid session.");
            }

            var code = _pkceService.GenerateAuthorizationCode();

            var authorizationCode = new AuthorizationCode
            {
                Id = Guid.NewGuid(),
                Code = code,
                CodeChallenge = request.CodeChallenge,
                CodeChallengeMethod = request.CodeChallengeMethod,
                UserId = user.Id,
                ApplicationId = application.Id,
                RedirectUri = request.RedirectUri,
                Scope = request.Scope,
                ExpiresAt = DateTime.UtcNow.AddSeconds(AuthorizationCodeExpirationSeconds),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.AuthorizationCodes.Add(authorizationCode);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Authorization code generated for user {UserId} on client {ClientId}",
                user.Id, application.ClientId);

            return new AuthorizeQueryResponse
            {
                RequiresAuthentication = false,
                RequiresConsent = false,
                AuthorizationCode = code,
                State = request.State,
                ExpiresIn = AuthorizationCodeExpirationSeconds
            };
        }

        private static bool IsValidRedirectUri(Application application, string redirectUri)
        {
            if (string.IsNullOrEmpty(application.AllowedRedirectUris))
            {
                return false;
            }

            var allowedUris = application.AllowedRedirectUris
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(u => u.Trim());

            return allowedUris.Contains(redirectUri, StringComparer.OrdinalIgnoreCase);
        }
    }
}
