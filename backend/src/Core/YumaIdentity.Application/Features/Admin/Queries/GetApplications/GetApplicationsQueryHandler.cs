namespace YumaIdentity.Application.Features.Admin.Queries.GetApplications
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Interfaces;

    public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, List<ApplicationResponse>>
    {
        private readonly IAppDbContext _context;

        public GetApplicationsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationResponse>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applications = await _context.Applications
                .AsNoTracking()
                .Select(app => new ApplicationResponse
                {
                    Id = app.Id,
                    AppName = app.AppName,
                    ClientId = app.ClientId,
                    AllowedRedirectUris = app.AllowedRedirectUris
                })
                .ToListAsync(cancellationToken);

            return applications;
        }
    }
}
