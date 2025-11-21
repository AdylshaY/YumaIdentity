namespace YumaIdentity.Application.Features.Admin.Commands.CreateApplication
{
    using MediatR;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Domain.Entities;

    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationRequest, CreateApplicationResponse>
    {
        private const string ValidAudiencesCacheKey = "ValidAudiences";
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMemoryCache _cache;

        public CreateApplicationCommandHandler(IAppDbContext context, IPasswordHasher passwordHasher, IMemoryCache cache)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _cache = cache;
        }

        public async Task<CreateApplicationResponse> Handle(CreateApplicationRequest request, CancellationToken cancellationToken)
        {
            var clientId = Guid.NewGuid().ToString("N");

            var secretBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(secretBytes);
            }
            var plainTextClientSecret = Convert.ToBase64String(secretBytes);

            var hashedClientSecret = _passwordHasher.HashPassword(plainTextClientSecret);

            var newApplication = new Application
            {
                Id = Guid.NewGuid(),
                AppName = request.AppName,
                ClientId = clientId,
                HashedClientSecret = hashedClientSecret,
                AllowedCallbackUrls = request.AllowedCallbackUrls,
                IsIsolated = request.IsIsolated
            };

            await _context.Applications.AddAsync(newApplication, cancellationToken);

            var adminRole = new AppRole
            {
                Id = Guid.NewGuid(),
                ApplicationId = newApplication.Id,
                RoleName = "Admin"
            };

            var userRole = new AppRole
            {
                Id = Guid.NewGuid(),
                ApplicationId = newApplication.Id,
                RoleName = "User"
            };

            await _context.AppRoles.AddRangeAsync([adminRole, userRole], cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            _cache.Remove(ValidAudiencesCacheKey);

            return new CreateApplicationResponse
            {
                Id = newApplication.Id,
                AppName = newApplication.AppName,
                ClientId = newApplication.ClientId,
                ClientSecret = plainTextClientSecret,
                AllowedCallbackUrls = newApplication.AllowedCallbackUrls
            };
        }
    }
}
