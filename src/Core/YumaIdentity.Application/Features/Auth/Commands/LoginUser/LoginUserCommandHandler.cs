namespace YumaIdentity.Application.Features.Auth.Commands.LoginUser
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
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

        public LoginUserCommandHandler(
            IAppDbContext context,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IClientValidator clientValidator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _clientValidator = clientValidator;
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
                throw new ValidationException("Invalid email or password.");

            if (!user.IsEmailVerified)
                throw new ValidationException("Please verify your email address before logging in.");

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
                    }

                    throw new ValidationException("No access rights and no default role found.");
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

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.TokenHash,
                AccessTokenExpiration = expirationTime
            };
        }
    }
}
