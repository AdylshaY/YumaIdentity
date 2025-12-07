namespace YumaIdentity.Application.Features.Admin.Commands.DeleteApplication
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;
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
