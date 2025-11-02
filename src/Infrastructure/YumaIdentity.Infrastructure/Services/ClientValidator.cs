using Microsoft.EntityFrameworkCore;
using YumaIdentity.Application.Common.Exceptions;
using YumaIdentity.Application.Interfaces;

namespace YumaIdentity.Infrastructure.Services
{
    public class ClientValidator : IClientValidator
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public ClientValidator(IAppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<Domain.Entities.Application> ValidateAndGetApplicationAsync(string clientId, string clientSecret)
        {
            var application = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ClientId == clientId);

            if (application == null)
                throw new NotFoundException("Application", clientId);

            if (!_passwordHasher.VerifyPassword(clientSecret, application.HashedClientSecret))
                throw new ValidationException("Invalid client credentials.");

            return application;
        }
    }
}
