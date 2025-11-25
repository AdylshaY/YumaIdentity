using MediatR;
using Microsoft.EntityFrameworkCore;
using YumaIdentity.Application.Common.Exceptions;
using YumaIdentity.Application.Interfaces;
using YumaIdentity.Domain.Enums;

namespace YumaIdentity.Application.Features.Auth.Commands.VerifyEmail
{
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
            var users = await _context.Users
                .Where(u => u.Email == request.Email && !u.IsEmailVerified)
                .Include(u => u.UserTokens)
                .ToListAsync(cancellationToken);

            if (users.Count == 0)
                throw new ValidationException("User not found or already verified.");

            foreach (var user in users)
            {
                var validToken = user.UserTokens.FirstOrDefault(t =>
                    t.TokenTypeId == (int)DomainTokenType.EmailVerify &&
                    !t.IsUsed &&
                    t.ExpiresAt > DateTime.UtcNow);

                if (validToken != null && _passwordHasher.VerifyPassword(request.Token, validToken.TokenHash))
                {
                    user.IsEmailVerified = true;
                    user.UpdatedAt = DateTime.UtcNow;
                    validToken.IsUsed = true;

                    await _context.SaveChangesAsync(cancellationToken);
                    return Unit.Value;
                }
            }

            throw new ValidationException("Invalid or expired email verification token.");
        }
    }
}
