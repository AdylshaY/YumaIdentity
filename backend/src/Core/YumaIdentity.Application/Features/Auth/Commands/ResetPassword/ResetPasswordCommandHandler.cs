using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YumaIdentity.Application.Common.Exceptions;
using YumaIdentity.Application.Interfaces;
using YumaIdentity.Domain.Enums;

namespace YumaIdentity.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordRequest, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public ResetPasswordCommandHandler(IAppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            Guid userId;
            string rawToken;

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
                t.TokenTypeId == (int)DomainTokenType.PasswordReset &&
                !t.IsUsed &&
                t.ExpiresAt > DateTime.UtcNow);

            if (validTokenRecord != null)
            {
                if (_passwordHasher.VerifyPassword(rawToken, validTokenRecord.TokenHash))
                {
                    user.HashedPassword = _passwordHasher.HashPassword(request.NewPassword);
                    _context.UserTokens.Remove(validTokenRecord);

                    await _context.SaveChangesAsync(cancellationToken);
                    return Unit.Value;
                }
            }

            throw new ValidationException("Invalid or expired password reset token.");
        }
    }
}
