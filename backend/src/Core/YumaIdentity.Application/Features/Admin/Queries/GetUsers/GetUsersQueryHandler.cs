namespace YumaIdentity.Application.Features.Admin.Queries.GetUsers
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Interfaces;

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<GetUsersResponse>>
    {
        private readonly IAppDbContext _context;

        public GetUsersQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .AsNoTracking()
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new GetUsersResponse
                {
                    Id = u.Id,
                    Email = u.Email,
                    IsEmailVerified = u.IsEmailVerified,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            return users;
        }
    }
}
