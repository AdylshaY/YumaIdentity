using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using YumaIdentity.Application.Interfaces;

namespace YumaIdentity.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                                 ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");

        public string? ClientId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("aud");

        public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    }
}
