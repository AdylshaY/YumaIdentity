using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using YumaIdentity.Application.Common.Exceptions;
using YumaIdentity.Application.Interfaces;
using YumaIdentity.Domain.Entities;
using YumaIdentity.Domain.Enums;

namespace YumaIdentity.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordRequest, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IClientValidator _clientValidator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ForgotPasswordCommandHandler(
            IAppDbContext context,
            IClientValidator clientValidator,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _context = context;
            _clientValidator = clientValidator;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            var application = await _clientValidator.ValidateAndGetApplicationAsync(request.ClientId, request.ClientSecret);
            if (application == null) throw new NotFoundException("Application", request.ClientId);

            Guid? targetTenantId = application.IsIsolated ? application.Id : null;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.TenantId == targetTenantId, cancellationToken);

            if (user == null) return Unit.Value;

            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            var rawToken = Convert.ToBase64String(randomBytes);

            var tokenHash = _passwordHasher.HashPassword(rawToken);

            var userToken = new UserToken
            {
                UserId = user.Id,
                TokenTypeId = (int)DomainTokenType.PasswordReset,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                IsUsed = false
            };

            _context.UserTokens.Add(userToken);
            await _context.SaveChangesAsync(cancellationToken);

            var tokenPayload = $"{user.Id}:{rawToken}";
            var secureToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenPayload));

            var baseUrl = _configuration["AdminSeed:AdminDashboardUrl"] ?? "http://localhost:3000";
            if (!string.IsNullOrEmpty(application.ClientBaseUrl))
            {
                baseUrl = application.ClientBaseUrl;
            }

            var resetLink = $"{baseUrl.TrimEnd('/')}/auth/reset-password?token={Uri.EscapeDataString(secureToken)}";

            var emailBody = $@"
                <h1>Password Reset Request</h1>
                <p>We received a request to reset your password for your {application.AppName} account.</p>
                <p>To reset your password, please click the button below:</p>
                <a href='{resetLink}' style='padding: 10px 20px; background-color: #dc3545; color: white; text-decoration: none; border-radius: 5px;'>Reset Password</a>
                <p>This link will expire in 1 hour.</p>
                <p>If you did not request a password reset, please ignore this email.</p>
                <p>Thank you,<br/>{application.AppName} Team</p>
                <p>Note: This email was generated automatically, please do not reply.</p>
                <p>This infrastructure is powered by YumaIdentity.</p>
            ";

            try
            {
                await _emailService.SendEmailAsync(user.Email, "Password Reset Request", emailBody);
            }
            catch
            {

            }

            return Unit.Value;
        }
    }
}
