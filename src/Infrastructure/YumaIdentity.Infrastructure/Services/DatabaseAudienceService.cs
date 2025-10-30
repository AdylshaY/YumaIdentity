using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using YumaIdentity.Application.Interfaces;
using YumaIdentity.Infrastructure.Persistence;

namespace YumaIdentity.Infrastructure.Services
{
    public class DatabaseAudienceService : IValidAudienceService
    {
        private const string ValidAudiencesCacheKey = "ValidAudiences";
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public DatabaseAudienceService(IMemoryCache cache, IServiceScopeFactory scopeFactory)
        {
            _cache = cache;
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> IsAudienceValidAsync(IEnumerable<string> audiences)
        {
            if (_cache.TryGetValue(ValidAudiencesCacheKey, out List<string> validClientIds))
            {
                return audiences.Any(aud => validClientIds.Contains(aud));
            }

            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                validClientIds = await LoadAudiencesFromDatabaseAsync(context);

                _cache.Set(ValidAudiencesCacheKey, validClientIds, TimeSpan.FromMinutes(5));

                return audiences.Any(aud => validClientIds.Contains(aud));
            }
        }

        private static async Task<List<string>> LoadAudiencesFromDatabaseAsync(AppDbContext context)
        {
            return await context.Applications
                                .Select(c => c.ClientId)
                                .ToListAsync();
        }
    }
}
