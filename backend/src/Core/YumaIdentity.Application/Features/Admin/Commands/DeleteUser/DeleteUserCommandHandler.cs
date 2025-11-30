namespace YumaIdentity.Application.Features.Admin.Commands.DeleteUser
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Interfaces;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly ILogger<DeleteUserCommandHandler> _logger;

        public DeleteUserCommandHandler(IAppDbContext context, ILogger<DeleteUserCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(
                [request.Id],
                cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Admin tried to delete non-existent User {UserId}.", request.Id);
                throw new NotFoundException("User", request.Id);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Admin deleted User {UserId} (Email: {Email}).", user.Id, user.Email);

            return Unit.Value;
        }
    }
}
