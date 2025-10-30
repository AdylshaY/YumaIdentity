namespace YumaIdentity.Application.Features.Auth.Commands.RefreshToken
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Application.Models;

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenRequest, TokenResponse>
    {
        private readonly IAppDbContext _context;
        private readonly ITokenGenerator _tokenGenerator;

        public RefreshTokenCommandHandler(IAppDbContext context, ITokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<TokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ClientId == request.ClientId, cancellationToken);

            if (application == null)
                throw new NotFoundException("Application", request.ClientId);

            var foundToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.TokenHash == request.RefreshToken, cancellationToken);

            if (foundToken == null)
                throw new ValidationException("Invalid refresh token.");

            if (foundToken.IsRevoked)
                throw new ValidationException("This refresh token has been revoked.");

            if (foundToken.ExpiresAt < DateTime.UtcNow)
                throw new ValidationException("Refresh token has expired.");

            if (foundToken.ApplicationId != application.Id)
                throw new ValidationException("Invalid refresh token for this application.");

            var user = foundToken.User;

            foundToken.IsRevoked = true;

            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.AppRoles, ur => ur.RoleId, ar => ar.Id, (ur, ar) => new { AppRole = ar })
                .Where(x => x.AppRole.ApplicationId == application.Id)
                .Select(x => x.AppRole.RoleName)
                .ToListAsync(cancellationToken);

            var newAccessToken = _tokenGenerator.GenerateAccessToken(user, application, userRoles);
            var newRefreshToken = _tokenGenerator.GenerateRefreshToken(user, application);

            _context.RefreshTokens.Add(newRefreshToken);

            await _context.SaveChangesAsync(cancellationToken);

            var expirationTime = DateTime.UtcNow.AddMinutes(_tokenGenerator.GetAccessTokenExpirationInMinutes());

            return new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.TokenHash,
                AccessTokenExpiration = expirationTime
            };
        }
    }
}
