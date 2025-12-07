namespace YumaIdentity.Application.Features.Admin.Queries.GetApplications
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using System.Collections.Generic;

    public class GetApplicationsQuery : IRequest<List<ApplicationResponse>>
    {
    }
}
