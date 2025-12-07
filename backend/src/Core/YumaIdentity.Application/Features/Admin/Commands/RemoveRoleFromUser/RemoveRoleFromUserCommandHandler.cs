namespace YumaIdentity.Application.Features.Admin.Commands.RemoveRoleFromUser
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Interfaces;

    public class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand, Unit>
    {
        private readonly IAppDbContext _context;

        public RemoveRoleFromUserCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
        {
            var userRoleAssignment = await _context.UserRoles.FindAsync(
                [request.UserId, request.RoleId],
                cancellationToken);

            if (userRoleAssignment == null)
                throw new NotFoundException($"No role assignment found for user {request.UserId} with role {request.RoleId}.");


            _context.UserRoles.Remove(userRoleAssignment);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
