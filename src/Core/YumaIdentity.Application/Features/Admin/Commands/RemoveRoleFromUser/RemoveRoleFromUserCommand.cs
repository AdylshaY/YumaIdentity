namespace YumaIdentity.Application.Features.Admin.Commands.RemoveRoleFromUser
{
    using MediatR;
    using System;

    public class RemoveRoleFromUserCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public RemoveRoleFromUserCommand(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
