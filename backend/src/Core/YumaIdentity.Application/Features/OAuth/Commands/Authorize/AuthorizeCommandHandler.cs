namespace YumaIdentity.Application.Features.OAuth.Commands.Authorize
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
    /// Handles the OAuth2 authorization request.
    /// Validates credentials and returns an authorization code for PKCE flow.
    /// </summary>
    public class AuthorizeCommandHandler : IRequestHandler<AuthorizeRequest, AuthorizeResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPkceService _pkceService;
        private readonly ILogger<AuthorizeCommandHandler> _logger;

        private const int AuthorizationCodeExpirationSeconds = 60;

        public AuthorizeCommandHandler(
            IAppDbContext context,
            IPasswordHasher passwordHasher,
            IPkceService pkceService,
            ILogger<AuthorizeCommandHandler> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _pkceService = pkceService;
            _logger = logger;
        }

        public async Task<AuthorizeResponse> Handle(AuthorizeRequest request, CancellationToken cancellationToken)
        {
            // 1. Validate the client application
            var application = await _context.Applications
                .FirstOrDefaultAsync(a => a.ClientId == request.ClientId, cancellationToken);

            if (application == null)
            {
                _logger.LogWarning("Authorization failed: Unknown client_id {ClientId}", request.ClientId);
                throw new NotFoundException("Application", request.ClientId);
            }

            // 2. For Public clients, PKCE is required and secret is not used
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

            // 3. Validate redirect_uri
            if (!IsValidRedirectUri(application, request.RedirectUri))
            {
                _logger.LogWarning("Authorization failed: Invalid redirect_uri {RedirectUri} for client {ClientId}",
                    request.RedirectUri, request.ClientId);
                throw new ValidationException("Invalid redirect_uri.");
            }

            // 4. Find the user based on tenant isolation
            Guid? targetTenantId = application.IsIsolated ? application.Id : null;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.TenantId == targetTenantId, cancellationToken);

            // 5. Validate credentials
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.HashedPassword))
            {
                _logger.LogWarning("Authorization failed: Invalid credentials for {Email} on client {ClientId}",
                    request.Email, request.ClientId);
                throw new ValidationException("Invalid email or password.");
            }

            // 6. Check email verification
            if (!user.IsEmailVerified)
            {
                _logger.LogWarning("Authorization blocked: Email not verified for {Email}", request.Email);
                throw new ValidationException("Please verify your email address before logging in.");
            }

            // 7. Generate authorization code
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

            return new AuthorizeResponse
            {
                AuthorizationCode = code,
                ExpiresIn = AuthorizationCodeExpirationSeconds
            };
        }

        /// <summary>
        /// Validates that the redirect_uri is in the application's allowed list.
        /// </summary>
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
