using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YumaIdentity.Application.Common.Exceptions;
using YumaIdentity.Application.Common.Interfaces.Mediator;
using YumaIdentity.Application.Interfaces;

namespace YumaIdentity.Application.Features.Management.Commands.RemoveAppUsers
{
    public class RemoveAppUserCommandHandler : IRequestHandler<RemoveAppUserCommand, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly ILogger<RemoveAppUserCommandHandler> _logger;

        public RemoveAppUserCommandHandler(IAppDbContext context, ILogger<RemoveAppUserCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveAppUserCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(a => a.ClientId == request.ClientId, cancellationToken) ?? throw new NotFoundException("Application", request.ClientId);

            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == request.UserId && ur.Role.ApplicationId == application.Id)
                .ToListAsync(cancellationToken);

            if (userRoles.Count == 0)
            {
                throw new NotFoundException("User is not a member of this application.");
            }

            _context.UserRoles.RemoveRange(userRoles);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tenant Admin removed User {UserId} from Application {ClientId}", request.UserId, request.ClientId);

            return Unit.Value;
        }
    }
}
