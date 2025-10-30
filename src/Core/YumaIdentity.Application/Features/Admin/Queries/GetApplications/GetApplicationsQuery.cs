namespace YumaIdentity.Application.Features.Admin.Queries.GetApplications
{
    using MediatR;
    using System.Collections.Generic;

    public class GetApplicationsQuery : IRequest<List<ApplicationResponse>>
    {
    }
}
