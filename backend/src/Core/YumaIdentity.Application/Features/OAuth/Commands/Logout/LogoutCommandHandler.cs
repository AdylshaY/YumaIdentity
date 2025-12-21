namespace YumaIdentity.Application.Features.OAuth.Commands.Logout
{
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Interfaces;

    /// <summary>
    /// Handles logout request by clearing the OAuth session.
    /// </summary>
    public class LogoutCommandHandler : IRequestHandler<LogoutRequest, Unit>
    {
        private readonly IOAuthSessionService _sessionService;
        private readonly ILogger<LogoutCommandHandler> _logger;

        public LogoutCommandHandler(
            IOAuthSessionService sessionService,
            ILogger<LogoutCommandHandler> logger)
        {
            _sessionService = sessionService;
            _logger = logger;
        }

        public Task<Unit> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            _sessionService.RemoveSession(request.SessionId);
            _logger.LogInformation("Session cleared: {SessionId}", request.SessionId);
            return Task.FromResult(Unit.Value);
        }
    }
}
