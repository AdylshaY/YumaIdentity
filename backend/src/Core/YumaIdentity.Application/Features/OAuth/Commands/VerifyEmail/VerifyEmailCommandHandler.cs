namespace YumaIdentity.Application.Features.OAuth.Commands.VerifyEmail
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Domain.Enums;

    /// <summary>
    /// Handles email verification using a valid verification token.
    /// </summary>
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailRequest, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public VerifyEmailCommandHandler(IAppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(VerifyEmailRequest request, CancellationToken cancellationToken)
        {
            string rawToken;
            Guid userId;

            try
            {
                var bytes = Convert.FromBase64String(request.Token);
                var decoded = Encoding.UTF8.GetString(bytes);
                var parts = decoded.Split(':');

                if (parts.Length != 2) throw new Exception();

                userId = Guid.Parse(parts[0]);
                rawToken = parts[1];
            }
            catch
            {
                throw new ValidationException("Invalid token format.");
            }

            var user = await _context.Users
                .Include(u => u.UserTokens)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null) throw new ValidationException("User not found.");

            var validTokenRecord = user.UserTokens.FirstOrDefault(t =>
                t.TokenTypeId == (int)DomainTokenType.EmailVerify &&
                !t.IsUsed &&
                t.ExpiresAt > DateTime.UtcNow);

            if (validTokenRecord != null)
            {
                if (_passwordHasher.VerifyPassword(rawToken, validTokenRecord.TokenHash))
                {
                    user.IsEmailVerified = true;
                    _context.UserTokens.Remove(validTokenRecord);

                    await _context.SaveChangesAsync(cancellationToken);
                    return Unit.Value;
                }
            }

            throw new ValidationException("Invalid or expired email verification token.");
        }
    }
}
