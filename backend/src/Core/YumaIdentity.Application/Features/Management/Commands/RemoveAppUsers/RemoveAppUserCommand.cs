using YumaIdentity.Application.Common.Interfaces.Mediator;

namespace YumaIdentity.Application.Features.Management.Commands.RemoveAppUsers
{
    public class RemoveAppUserCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public string ClientId { get; set; }

        public RemoveAppUserCommand(Guid userId, string clientId)
        {
            UserId = userId;
            ClientId = clientId;
        }
    }
}
