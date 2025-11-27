namespace YumaIdentity.Infrastructure.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Domain.Entities;
    using YumaIdentity.Infrastructure.Persistence;

    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public DatabaseSeeder(IAppDbContext context, IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task InitializeAsync()
        {
            if (_context is AppDbContext dbContext)
            {
                await dbContext.Database.MigrateAsync();
            }

            var adminClientId = _configuration["AdminSeed:AdminClientId"];

            if (!await _context.Applications.AnyAsync(a => a.ClientId == adminClientId))
            {
                var adminClientBaseUrl = _configuration["AdminSeed:AdminDashboardUrl"];

                var adminApp = new Application
                {
                    Id = Guid.NewGuid(),
                    AppName = _configuration["AdminSeed:AdminClientName"]!,
                    ClientId = adminClientId!,
                    HashedClientSecret = _passwordHasher.HashPassword(_configuration["AdminSeed:AdminClientSecret"]!),
                    AllowedCallbackUrls = $"[\"{adminClientBaseUrl}\"]",
                    ClientBaseUrl = adminClientBaseUrl,
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
                    Email = _configuration["AdminSeed:SuperAdminEmail"]!,
                    HashedPassword = _passwordHasher.HashPassword(_configuration["AdminSeed:SuperAdminPassword"]!),
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
            }
        }
    }
}
