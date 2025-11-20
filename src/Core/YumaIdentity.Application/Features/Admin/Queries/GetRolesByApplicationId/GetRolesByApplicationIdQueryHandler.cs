using MediatR;
using Microsoft.EntityFrameworkCore;
using YumaIdentity.Application.Interfaces;

namespace YumaIdentity.Application.Features.Admin.Queries.GetRolesByApplicationId
{
  public class GetRolesByApplicationIdQueryHandler : IRequestHandler<GetRolesByApplicationIdQuery, List<GetAppRolesResponse>>
  {
    private readonly IAppDbContext _context;

    public GetRolesByApplicationIdQueryHandler(IAppDbContext context)
    {
      _context = context;
    }

    public async Task<List<GetAppRolesResponse>> Handle(GetRolesByApplicationIdQuery request, CancellationToken cancellationToken)
    {
      return await _context.AppRoles
          .AsNoTracking()
          .Where(r => r.ApplicationId == request.ApplicationId)
          .Select(r => new GetAppRolesResponse
          {
            RoleId = r.Id,
            RoleName = r.RoleName
          })
          .ToListAsync(cancellationToken);
    }
  }
}