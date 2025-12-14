namespace YumaIdentity.Application.Interfaces
{
    using YumaIdentity.Domain.Entities;

    /// <summary>
    /// Validates OAuth2 client applications.
    /// </summary>
    public interface IClientValidator
    {
        /// <summary>
        /// Validates a client application with credentials (for Confidential clients).
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret (required for Confidential clients, null for Public clients).</param>
        /// <returns>The validated application.</returns>
        Task<Application> ValidateAndGetApplicationAsync(string? clientId, string? clientSecret);

        /// <summary>
        /// Gets an application by ClientId without validating credentials.
        /// Use this for Public clients or endpoints that don't require secret validation.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns>The application if found.</returns>
        Task<Application?> GetApplicationByClientIdAsync(string clientId);
    }
}
