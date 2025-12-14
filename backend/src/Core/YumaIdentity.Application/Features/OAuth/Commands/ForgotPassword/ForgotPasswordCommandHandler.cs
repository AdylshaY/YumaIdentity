namespace YumaIdentity.Application.Features.OAuth.Commands.ForgotPassword
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Domain.Entities;
    using YumaIdentity.Domain.Enums;

    /// <summary>
    /// Handles the forgot password request.
    /// Generates a password reset token and sends it to the user's email.
    /// </summary>
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordRequest, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IClientValidator _clientValidator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;

        public ForgotPasswordCommandHandler(
            IAppDbContext context,
            IClientValidator clientValidator,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            IConfiguration configuration,
            ILogger<ForgotPasswordCommandHandler> logger)
        {
            _context = context;
            _clientValidator = clientValidator;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Unit> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            var application = await _clientValidator.ValidateAndGetApplicationAsync(request.ClientId, request.ClientSecret);
            if (application == null) throw new NotFoundException("Application", request.ClientId ?? "unknown");

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
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send password reset email to {Email}", user.Email);
            }

            return Unit.Value;
        }
    }
}
