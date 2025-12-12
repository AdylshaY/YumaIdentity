namespace YumaIdentity.Application.Features.OAuth.Commands.Token
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Features.OAuth.Shared;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Domain.Entities;
    using YumaIdentity.Domain.Enums;

    /// <summary>
    /// Handles the OAuth2 token request.
    /// Exchanges authorization codes for tokens or refreshes access tokens.
    /// </summary>
    public class TokenCommandHandler : IRequestHandler<TokenRequest, TokenResponse>
    {
        private readonly IAppDbContext _context;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IPkceService _pkceService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<TokenCommandHandler> _logger;

        public TokenCommandHandler(
            IAppDbContext context,
            ITokenGenerator tokenGenerator,
            IPkceService pkceService,
            IPasswordHasher passwordHasher,
            ILogger<TokenCommandHandler> logger)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
            _pkceService = pkceService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<TokenResponse> Handle(TokenRequest request, CancellationToken cancellationToken)
        {
            return request.GrantType.ToLowerInvariant() switch
            {
                "authorization_code" => await HandleAuthorizationCodeGrant(request, cancellationToken),
                "refresh_token" => await HandleRefreshTokenGrant(request, cancellationToken),
                _ => throw new ValidationException($"Unsupported grant_type: {request.GrantType}")
            };
        }

        private async Task<TokenResponse> HandleAuthorizationCodeGrant(TokenRequest request, CancellationToken cancellationToken)
        {
            // 1. Validate required fields
            if (string.IsNullOrEmpty(request.Code))
                throw new ValidationException("Authorization code is required.");

            if (string.IsNullOrEmpty(request.RedirectUri))
                throw new ValidationException("Redirect URI is required.");

            // 2. Find the authorization code
            var authCode = await _context.AuthorizationCodes
                .Include(ac => ac.User)
                .Include(ac => ac.Application)
                .FirstOrDefaultAsync(ac => ac.Code == request.Code, cancellationToken);

            if (authCode == null)
            {
                _logger.LogWarning("Token request failed: Invalid authorization code");
                throw new ValidationException("Invalid authorization code.");
            }

            // 3. Validate the authorization code
            if (authCode.IsUsed)
            {
                _logger.LogWarning("Token request failed: Authorization code already used for user {UserId}", authCode.UserId);
                throw new ValidationException("Authorization code has already been used.");
            }

            if (authCode.ExpiresAt < DateTime.UtcNow)
            {
                _logger.LogWarning("Token request failed: Authorization code expired for user {UserId}", authCode.UserId);
                throw new ValidationException("Authorization code has expired.");
            }

            if (!string.Equals(authCode.RedirectUri, request.RedirectUri, StringComparison.Ordinal))
            {
                _logger.LogWarning("Token request failed: Redirect URI mismatch");
                throw new ValidationException("Redirect URI does not match.");
            }

            if (!string.Equals(authCode.Application.ClientId, request.ClientId, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Token request failed: Client ID mismatch");
                throw new ValidationException("Client ID does not match.");
            }

            // 4. Validate client credentials or PKCE
            var application = authCode.Application;
            if (application.ClientType == ClientType.Confidential)
            {
                // Confidential clients must provide client_secret
                if (string.IsNullOrEmpty(request.ClientSecret) ||
                    !_passwordHasher.VerifyPassword(request.ClientSecret, application.HashedClientSecret!))
                {
                    throw new ValidationException("Invalid client credentials.");
                }
            }
            else
            {
                // Public clients must provide code_verifier for PKCE
                if (string.IsNullOrEmpty(request.CodeVerifier))
                {
                    throw new ValidationException("Code verifier is required for public clients.");
                }

                if (!_pkceService.ValidateCodeChallenge(request.CodeVerifier, authCode.CodeChallenge, authCode.CodeChallengeMethod))
                {
                    _logger.LogWarning("Token request failed: PKCE validation failed for user {UserId}", authCode.UserId);
                    throw new ValidationException("Code verifier validation failed.");
                }
            }

            // 5. Mark authorization code as used
            authCode.IsUsed = true;
            await _context.SaveChangesAsync(cancellationToken);

            // 6. Generate tokens
            return await GenerateTokens(authCode.User, application, cancellationToken);
        }

        private async Task<TokenResponse> HandleRefreshTokenGrant(TokenRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                throw new ValidationException("Refresh token is required.");

            // Find the refresh token
            var refreshToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .Include(rt => rt.Application)
                .FirstOrDefaultAsync(rt => rt.TokenHash == request.RefreshToken, cancellationToken);

            if (refreshToken == null)
            {
                _logger.LogWarning("Token refresh failed: Invalid refresh token");
                throw new ValidationException("Invalid refresh token.");
            }

            if (refreshToken.IsRevoked)
            {
                _logger.LogWarning("Token refresh failed: Refresh token has been revoked for user {UserId}", refreshToken.UserId);
                throw new ValidationException("Refresh token has been revoked.");
            }

            if (refreshToken.ExpiresAt < DateTime.UtcNow)
            {
                _logger.LogWarning("Token refresh failed: Refresh token expired for user {UserId}", refreshToken.UserId);
                throw new ValidationException("Refresh token has expired.");
            }

            if (!string.Equals(refreshToken.Application.ClientId, request.ClientId, StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidationException("Client ID does not match.");
            }

            // Rotate refresh token (revoke old one, create new one)
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;

            return await GenerateTokens(refreshToken.User, refreshToken.Application, cancellationToken);
        }

        private async Task<TokenResponse> GenerateTokens(User user, Application application, CancellationToken cancellationToken)
        {
            // Get user roles for this application
            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.AppRoles,
                      ur => ur.RoleId,
                      ar => ar.Id,
                      (ur, ar) => new { AppRole = ar })
                .Where(x => x.AppRole.ApplicationId == application.Id)
                .Select(x => x.AppRole.RoleName)
                .ToListAsync(cancellationToken);

            // Auto-provision default "User" role if needed (for global applications)
            if (!application.IsIsolated && !userRoles.Any())
            {
                var defaultRole = await _context.AppRoles
                    .FirstOrDefaultAsync(r => r.ApplicationId == application.Id && r.RoleName == "User", cancellationToken);

                if (defaultRole != null)
                {
                    var newUserRole = new UserRole { UserId = user.Id, RoleId = defaultRole.Id };
                    _context.UserRoles.Add(newUserRole);
                    await _context.SaveChangesAsync(cancellationToken);
                    userRoles.Add(defaultRole.RoleName);
                    _logger.LogInformation("Auto-provisioned 'User' role for {UserId} on {ClientId}", user.Id, application.ClientId);
                }
            }

            // Generate tokens
            var accessToken = _tokenGenerator.GenerateAccessToken(user, application, userRoles);
            var refreshToken = _tokenGenerator.GenerateRefreshToken(user, application);

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            var expiresInMinutes = _tokenGenerator.GetAccessTokenExpirationInMinutes();

            _logger.LogInformation("Tokens generated for user {UserId} on {ClientId}", user.Id, application.ClientId);

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.TokenHash,
                ExpiresIn = expiresInMinutes * 60
            };
        }
    }
}
