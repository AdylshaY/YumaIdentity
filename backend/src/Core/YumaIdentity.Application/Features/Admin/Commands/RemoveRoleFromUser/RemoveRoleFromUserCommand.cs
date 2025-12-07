namespace YumaIdentity.Application.Features.Admin.Commands.RemoveRoleFromUser
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;
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
