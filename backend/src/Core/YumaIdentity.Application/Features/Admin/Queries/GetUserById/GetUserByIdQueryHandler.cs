namespace YumaIdentity.Application.Features.Admin.Queries.GetUserById
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Interfaces;

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
    {
        private readonly IAppDbContext _context;

        public GetUserByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(ar => ar.Application)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("User", request.Id);
            }

            var response = new GetUserByIdResponse
            {
                Id = user.Id,
                Email = user.Email,
                IsEmailVerified = user.IsEmailVerified,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Roles = user.UserRoles.Select(ur => new UserRoleMembershipDto
                {
                    RoleId = ur.RoleId,
                    RoleName = ur.Role.RoleName,
                    ApplicationId = ur.Role.ApplicationId,
                    ApplicationName = ur.Role.Application.AppName
                }).ToList()
            };

            return response;
        }
    }
}
