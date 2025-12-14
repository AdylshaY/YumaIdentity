namespace YumaIdentity.Application.Features.OAuth.Commands.RegisterUser
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
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
    /// Handles user registration.
    /// Creates a new user and sends an email verification link.
    /// </summary>
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserRequest, Guid>
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IClientValidator _clientValidator;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(
            IAppDbContext context,
            IPasswordHasher passwordHasher,
            IClientValidator clientValidator,
            IEmailService emailService,
            IConfiguration configuration,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _clientValidator = clientValidator;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Guid> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var application = await _clientValidator.ValidateAndGetApplicationAsync(request.ClientId, request.ClientSecret);

            if (application == null) throw new NotFoundException("Application", request.ClientId ?? "unknown");

            Guid? targetTenantId = application.IsIsolated ? application.Id : null;

            var userExists = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.TenantId == targetTenantId, cancellationToken);

            if (userExists != null)
            {
                if (!application.IsIsolated)
                {
                    bool hasRole = userExists.UserRoles.Any(ur => ur.Role.ApplicationId == application.Id);
                    if (hasRole) throw new ValidationException("User already registered for this application.");
                    throw new ValidationException("You have a Global Account. Please login to connect.");
                }

                throw new ValidationException("Email already in use in this application.");
            }

            var defaultRole = await _context.AppRoles
                    .FirstOrDefaultAsync(r => r.ApplicationId == application.Id && r.RoleName == "User", cancellationToken);

            if (defaultRole == null) throw new ValidationException("Default 'User' role not configured for this application. Please seed the database.");

            var hashedPassword = _passwordHasher.HashPassword(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                HashedPassword = hashedPassword,
                IsEmailVerified = false,
                CreatedAt = DateTime.UtcNow,
                TenantId = targetTenantId
            };

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = defaultRole.Id
            };

            _context.Users.Add(user);
            _context.UserRoles.Add(userRole);

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
                TokenTypeId = (int)DomainTokenType.EmailVerify,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                IsUsed = false
            };
            _context.UserTokens.Add(userToken);
            await _context.SaveChangesAsync(cancellationToken);

            var tokenPayload = $"{user.Id}:{rawToken}";
            var secureToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tokenPayload));

            var baseUrl = _configuration["AdminSeed:AdminDashboardUrl"] ?? "http://localhost:3000";
            if (!string.IsNullOrEmpty(application.ClientBaseUrl))
            {
                baseUrl = application.ClientBaseUrl;
            }

            var verifyLink = $"{baseUrl.TrimEnd('/')}/auth/verify-email?token={Uri.EscapeDataString(secureToken)}";

            var emailBody = $@"
                    <h1>Welcome!</h1>
                    <p>Thank you for creating your {application.AppName} account.</p>
                    <p>To verify your account, please click the button below:</p>
                    <a href='{verifyLink}' style='padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>Verify My Account</a>
                    <p>Or use this code: <b>{secureToken}</b></p>
                    <p>This link will expire in 1 hour.</p>
                    <p>Thank you,<br/>{application.AppName} Team</p>
                    <p>Note: This email was generated automatically, please do not reply.</p>
                    <p>This infrastructure is powered by YumaIdentity.</p>
                ";

            try
            {
                await _emailService.SendEmailAsync(user.Email, "Email Verification", emailBody);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send verification email to {Email}", user.Email);
            }

            return user.Id;
        }
    }
}
