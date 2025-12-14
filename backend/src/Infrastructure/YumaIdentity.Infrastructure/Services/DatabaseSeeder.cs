namespace YumaIdentity.Infrastructure.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Domain.Entities;
    using YumaIdentity.Domain.Enums;
    using YumaIdentity.Infrastructure.Persistence;

    /// <summary>
    /// Seeds the database with initial data (Admin application, SuperAdmin user).
    /// </summary>
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(
            IAppDbContext context,
            IPasswordHasher passwordHasher,
            IConfiguration configuration,
            ILogger<DatabaseSeeder> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            if (_context is AppDbContext dbContext)
            {
                await dbContext.Database.MigrateAsync();
            }

            var adminClientId = _configuration["AdminSeed:AdminClientId"] ?? "admin-dashboard";
            var adminClientName = _configuration["AdminSeed:AdminClientName"] ?? "YumaIdentity Admin Dashboard";
            var adminDashboardUrl = _configuration["AdminSeed:AdminDashboardUrl"] ?? "http://localhost:5173";
            var superAdminEmail = _configuration["AdminSeed:SuperAdminEmail"] ?? "superadmin@yumaidentity.local";
            var superAdminPassword = _configuration["AdminSeed:SuperAdminPassword"] ?? "SuperAdmin123!";

            if (!await _context.Applications.AnyAsync(a => a.ClientId == adminClientId))
            {
                _logger.LogInformation("Seeding admin application with ClientId: {ClientId}", adminClientId);

                var adminApp = new Application
                {
                    Id = Guid.NewGuid(),
                    AppName = adminClientName,
                    ClientId = adminClientId,
                    // Public client - no secret needed, uses PKCE
                    HashedClientSecret = null,
                    ClientType = ClientType.Public,
                    // AllowedCallbackUrls - comma separated for legacy compatibility
                    AllowedCallbackUrls = adminDashboardUrl,
                    // AllowedRedirectUris - comma separated for OAuth2 PKCE
                    AllowedRedirectUris = $"{adminDashboardUrl}/auth/callback,{adminDashboardUrl}/dashboard",
                    ClientBaseUrl = adminDashboardUrl,
                };

                await _context.Applications.AddAsync(adminApp);

                var superAdminRole = new AppRole
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = adminApp.Id,
                    RoleName = "SuperAdmin"
                };
                await _context.AppRoles.AddAsync(superAdminRole);

                var userRole = new AppRole
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = adminApp.Id,
                    RoleName = "User"
                };
                await _context.AppRoles.AddAsync(userRole);

                var superAdminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = superAdminEmail,
                    HashedPassword = _passwordHasher.HashPassword(superAdminPassword),
                    IsEmailVerified = true,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.Users.AddAsync(superAdminUser);

                await _context.UserRoles.AddAsync(new UserRole
                {
                    UserId = superAdminUser.Id,
                    RoleId = superAdminRole.Id
                });

                await _context.SaveChangesAsync(CancellationToken.None);

                _logger.LogInformation("Database seeded successfully. SuperAdmin: {Email}", superAdminEmail);
            }
            else
            {
                _logger.LogInformation("Database already seeded, skipping.");
            }
        }
    }
}
