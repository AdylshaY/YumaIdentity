namespace YumaIdentity.Application.Features.Admin.Commands.AssignRoleToUser
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Domain.Entities;

    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, Unit>
    {
        private readonly IAppDbContext _context;

        public AssignRoleToUserCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists)
                throw new NotFoundException("User", request.UserId);


            var roleExists = await _context.AppRoles
                .AnyAsync(r => r.Id == request.RoleId, cancellationToken);

            if (!roleExists)
                throw new NotFoundException("AppRole", request.RoleId);


            var existingAssignment = await _context.UserRoles
                .AnyAsync(ur => ur.UserId == request.UserId && ur.RoleId == request.RoleId, cancellationToken);

            if (existingAssignment)
                throw new ValidationException("User already has this role.");


            var newUserRole = new UserRole
            {
                UserId = request.UserId,
                RoleId = request.RoleId
            };

            await _context.UserRoles.AddAsync(newUserRole, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
