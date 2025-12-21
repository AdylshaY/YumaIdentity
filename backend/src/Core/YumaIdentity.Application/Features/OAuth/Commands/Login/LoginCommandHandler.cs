namespace YumaIdentity.Application.Features.OAuth.Commands.Login
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Interfaces;

    /// <summary>
    /// Handles internal login for OAuth UI.
    /// Validates credentials and creates an authentication session.
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IOAuthSessionService _sessionService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IAppDbContext context,
            IPasswordHasher passwordHasher,
            IOAuthSessionService sessionService,
            ILogger<LoginCommandHandler> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _sessionService = sessionService;
            _logger = logger;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(a => a.ClientId == request.ClientId, cancellationToken);

            if (application == null)
            {
                _logger.LogWarning("Login failed: Unknown client_id {ClientId}", request.ClientId);
                throw new NotFoundException("Application", request.ClientId);
            }

            Guid? targetTenantId = application.IsIsolated ? application.Id : null;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.TenantId == targetTenantId, cancellationToken);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.HashedPassword))
            {
                _logger.LogWarning("Login failed: Invalid credentials for {Email} on client {ClientId}",
                    request.Email, request.ClientId);
                throw new ValidationException("Invalid email or password.");
            }

            if (!user.IsEmailVerified)
            {
                _logger.LogWarning("Login blocked: Email not verified for {Email}", request.Email);
                throw new ValidationException("Please verify your email address before logging in.");
            }

            var sessionId = Guid.NewGuid().ToString();
            _sessionService.CreateSession(user.Id, sessionId);

            _logger.LogInformation("Session created for user {UserId} on client {ClientId}", user.Id, application.ClientId);

            return new LoginResponse
            {
                SessionId = sessionId,
                Email = user.Email
            };
        }
    }
}
