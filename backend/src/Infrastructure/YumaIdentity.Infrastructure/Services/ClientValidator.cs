namespace YumaIdentity.Infrastructure.Services
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Domain.Entities;
    using YumaIdentity.Domain.Enums;

    /// <summary>
    /// Validates OAuth2 client applications based on client type.
    /// </summary>
    public class ClientValidator : IClientValidator
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public ClientValidator(IAppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Validates a client application with credentials.
        /// - Confidential clients: ClientId + ClientSecret required
        /// - Public clients: Only ClientId required (secret ignored)
        /// </summary>
        public async Task<Application> ValidateAndGetApplicationAsync(string? clientId, string? clientSecret)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ValidationException("ClientId is required.");

            var application = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ClientId == clientId);

            if (application == null)
                throw new NotFoundException("Application", clientId);

            // For Confidential clients, validate secret
            if (application.ClientType == ClientType.Confidential)
            {
                if (string.IsNullOrEmpty(clientSecret))
                    throw new ValidationException("ClientSecret is required for confidential clients.");

                if (string.IsNullOrEmpty(application.HashedClientSecret) ||
                    !_passwordHasher.VerifyPassword(clientSecret, application.HashedClientSecret))
                {
                    throw new ValidationException("Invalid client credentials.");
                }
            }
            // For Public clients, no secret validation needed

            return application;
        }

        /// <summary>
        /// Gets an application by ClientId without validating credentials.
        /// Use for Public clients or endpoints that don't require secret validation.
        /// </summary>
        public async Task<Application?> GetApplicationByClientIdAsync(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                return null;

            return await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ClientId == clientId);
        }
    }
}
