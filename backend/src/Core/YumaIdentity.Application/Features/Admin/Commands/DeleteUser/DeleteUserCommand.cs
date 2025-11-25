namespace YumaIdentity.Application.Features.Admin.Commands.DeleteUser
{
    using MediatR;
    using System;

    public class DeleteUserCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }

        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }
    }
}
