namespace YumaIdentity.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using YumaIdentity.Domain.Enums;

    public class Application
    {
        public Guid Id { get; set; }
        public required string AppName { get; set; }
        public required string ClientId { get; set; }

        /// <summary>
        /// Hashed client secret. Nullable for Public clients that use PKCE.
        /// Required for Confidential clients.
        /// </summary>
        public string? HashedClientSecret { get; set; }

        /// <summary>
        /// Comma-separated list of allowed OAuth2 redirect URIs.
        /// Required for Public clients using the authorization code flow.
        /// </summary>
        public string? AllowedRedirectUris { get; set; }

        public string? ClientBaseUrl { get; set; }

        /// <summary>
        /// If true, this application has its own isolated user pool (multi-tenant).
        /// If false, users are shared across applications (global).
        /// </summary>
        public bool IsIsolated { get; set; } = false;

        /// <summary>
        /// The type of OAuth2 client.
        /// Confidential: Backend apps using ClientSecret.
        /// Public: SPA/Mobile apps using PKCE.
        /// </summary>
        public ClientType ClientType { get; set; } = ClientType.Confidential;

        public virtual ICollection<AppRole> AppRoles { get; set; } = [];
    }
}
