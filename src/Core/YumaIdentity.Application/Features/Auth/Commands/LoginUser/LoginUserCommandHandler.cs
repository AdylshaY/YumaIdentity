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

    public class LoginUserCommandHandler : IRequestHandler<LoginRequest, TokenResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public LoginUserCommandHandler(
            IAppDbContext context,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<TokenResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ClientId == request.ClientId, cancellationToken);

            if (application == null)
                throw new NotFoundException("Application", request.ClientId);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null)
                throw new ValidationException("Invalid email or password.");

            if (!_passwordHasher.VerifyPassword(request.Password, user.HashedPassword))
                throw new ValidationException("Invalid email or password.");

            if (!user.IsEmailVerified)
                throw new ValidationException("Please verify your email address before logging in.");

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
