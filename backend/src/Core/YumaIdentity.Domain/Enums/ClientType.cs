namespace YumaIdentity.Domain.Enums
{
    /// <summary>
    /// Defines the type of OAuth2 client application.
    /// </summary>
    public enum ClientType
    {
        /// <summary>
        /// Confidential clients are backend applications that can securely store a client secret.
        /// They use ClientId + ClientSecret for authentication.
        /// Example: Server-to-server integrations, backend services.
        /// </summary>
        Confidential = 0,

        /// <summary>
        /// Public clients are applications that cannot securely store a client secret.
        /// They use ClientId + PKCE (Proof Key for Code Exchange) for authentication.
        /// Example: Single Page Applications (SPA), Mobile apps, Desktop apps.
        /// </summary>
        Public = 1
    }
}
