namespace YumaIdentity.Infrastructure.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Infrastructure.Options;

    /// <summary>
    /// Service for dynamically validating CORS origins based on client configuration.
    /// </summary>
    public class DynamicCorsService
    {
        private readonly IAppDbContext _context;
        private readonly IOptions<CorsSettings> _corsSettings;
        private readonly ILogger<DynamicCorsService> _logger;

        public DynamicCorsService(
            IAppDbContext context,
            IOptions<CorsSettings> corsSettings,
            ILogger<DynamicCorsService> logger)
        {
            _context = context;
            _corsSettings = corsSettings;
            _logger = logger;
        }

        /// <summary>
        /// Validates if the given origin is allowed.
        /// Checks both trusted origins (appsettings) and client-specific origins (database).
        /// </summary>
        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            if (string.IsNullOrWhiteSpace(origin))
                return false;

            // Check trusted origins from appsettings (first-party apps)
            var trustedOrigins = _corsSettings.Value.TrustedOrigins;
            if (trustedOrigins != null && trustedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase))
            {
                _logger.LogDebug("Origin {Origin} allowed from trusted list", origin);
                return true;
            }

            // Check client-specific origins from database (third-party apps)
            var isAllowed = await _context.Applications
                .AnyAsync(a => a.AllowedOrigins != null && 
                              a.AllowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(o => o.Trim())
                                  .Contains(origin, StringComparer.OrdinalIgnoreCase));

            if (isAllowed)
            {
                _logger.LogDebug("Origin {Origin} allowed from client configuration", origin);
            }
            else
            {
                _logger.LogWarning("Origin {Origin} rejected - not in trusted or client lists", origin);
            }

            return isAllowed;
        }
    }
}
