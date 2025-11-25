using MediatR;

namespace YumaIdentity.Application.Features.Admin.Queries.GetRolesByApplicationId
{
    public class GetRolesByApplicationIdQuery : IRequest<List<GetAppRolesResponse>>
    {
        public Guid ApplicationId { get; set; }

        public GetRolesByApplicationIdQuery(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}