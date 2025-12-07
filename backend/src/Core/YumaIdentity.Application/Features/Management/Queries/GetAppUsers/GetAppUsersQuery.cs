using YumaIdentity.Application.Common.Interfaces.Mediator;

namespace YumaIdentity.Application.Features.Management.Queries.GetAppUsers
{
    public class GetAppUsersQuery : IRequest<List<GetAppUsersResponse>>
    {
        public string ClientId { get; set; }

        public GetAppUsersQuery(string clientId)
        {
            ClientId = clientId;
        }
    }
}
