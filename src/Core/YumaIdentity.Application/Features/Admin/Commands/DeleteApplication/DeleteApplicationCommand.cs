namespace YumaIdentity.Application.Features.Admin.Commands.DeleteApplication
{
    using MediatR;
    using System;

    public class DeleteApplicationCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }

        public DeleteApplicationCommand(Guid id)
        {
            Id = id;
        }
    }
}
