using Microsoft.EntityFrameworkCore;
using YumaIdentity.Application.Common.Interfaces.Mediator;
using YumaIdentity.Application.Interfaces;

namespace YumaIdentity.Application.Features.Management.Queries.GetAppUsers
{
    public class GetAppUsersQueryHandler : IRequestHandler<GetAppUsersQuery, List<GetAppUsersResponse>>
    {
        private readonly IAppDbContext _context;

        public GetAppUsersQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAppUsersResponse>> Handle(GetAppUsersQuery request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ClientId == request.ClientId, cancellationToken);

            if (application == null) return new List<GetAppUsersResponse>();

            var users = await _context.Users
                .AsNoTracking()
                .Where(u => u.UserRoles.Any(ur => ur.Role.ApplicationId == application.Id))
                .Select(u => new GetAppUsersResponse
                {
                    Id = u.Id,
                    Email = u.Email,
                    IsEmailVerified = u.IsEmailVerified,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return users;
        }
    }
}
