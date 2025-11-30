namespace YumaIdentity.Application.Features.Auth.Commands.LoginUser
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Features.Auth.Shared;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Domain.Entities;

    public class LoginUserCommandHandler : IRequestHandler<LoginRequest, TokenResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IClientValidator _clientValidator;
        private readonly ILogger<LoginUserCommandHandler> _logger;

        public LoginUserCommandHandler(
            IAppDbContext context,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IClientValidator clientValidator,
            ILogger<LoginUserCommandHandler> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _clientValidator = clientValidator;
            _logger = logger;
        }

        public async Task<TokenResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var application = await _clientValidator.ValidateAndGetApplicationAsync(request.ClientId, request.ClientSecret);

            if (application == null)
                throw new NotFoundException("Application", request.ClientId);

            Guid? targetTenantId = application.IsIsolated ? application.Id : null;

            var user = await _context.Users
                         .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                         .FirstOrDefaultAsync(u => u.Email == request.Email && u.TenantId == targetTenantId, cancellationToken);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.HashedPassword))
            {
                _logger.LogWarning("Failed login attempt for email: {Email} in application: {ApplicationId}", request.Email, application.Id);
                throw new ValidationException("Invalid email or password.");
            }

            if (!user.IsEmailVerified)
            {
                _logger.LogWarning("Login blocked for unverified user {Email}.", request.Email);
                throw new ValidationException("Please verify your email address before logging in.");
            }

            if (!application.IsIsolated)
            {
                var hasRoleInApp = user.UserRoles.Any(ur => ur.Role.ApplicationId == application.Id);
                if (!hasRoleInApp)
                {
                    var defaultRole = await _context.AppRoles
                        .FirstOrDefaultAsync(r => r.ApplicationId == application.Id && r.RoleName == "User", cancellationToken);

                    if (defaultRole != null)
                    {
                        var newRole = new UserRole { UserId = user.Id, RoleId = defaultRole.Id };
                        _context.UserRoles.Add(newRole);
                        await _context.SaveChangesAsync(cancellationToken);

                        newRole.Role = defaultRole;
                        user.UserRoles.Add(newRole);
                        _logger.LogInformation("Auto-provisioned 'User' role for {UserId} on Application {ClientId}.", user.Id, application.ClientId);
                    }
                    else
                    {
                        _logger.LogError("Login failed: Default 'User' role not found for Application {ClientId}.", application.ClientId);
                        throw new ValidationException("No access rights and no default role found.");
                    }
                }
            }

            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.AppRoles,
                      ur => ur.RoleId,
                      ar => ar.Id,
                      (ur, ar) => new { AppRole = ar })
                .Where(x => x.AppRole.ApplicationId == application.Id)
                .Select(x => x.AppRole.RoleName)
                .ToListAsync(cancellationToken);

            var accessToken = _tokenGenerator.GenerateAccessToken(user, application, userRoles);
            var refreshToken = _tokenGenerator.GenerateRefreshToken(user, application);

            var expirationInMinutes = _tokenGenerator.GetAccessTokenExpirationInMinutes();
            var expirationTime = DateTime.UtcNow.AddMinutes(expirationInMinutes);

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {UserId} successfully logged in to {ClientId}.", user.Id, application.ClientId);

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.TokenHash,
                AccessTokenExpiration = expirationTime
            };
        }
    }
}
