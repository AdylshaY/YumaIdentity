namespace YumaIdentity.Application.Features.Admin.Commands.AssignRoleToUser
{
    using MediatR;
    using System;

    public class AssignRoleToUserCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
