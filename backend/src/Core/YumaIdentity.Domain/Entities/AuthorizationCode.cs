namespace YumaIdentity.Domain.Entities
{
    using System;

    /// <summary>
    /// Represents an OAuth2 authorization code used in the PKCE flow.
    /// This is a short-lived, single-use code that is exchanged for tokens.
    /// </summary>
    public class AuthorizationCode
    {
        public Guid Id { get; set; }

        /// <summary>
        /// The authorization code string (random, cryptographically secure).
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// The PKCE code challenge (SHA256 hash of the code verifier).
        /// </summary>
        public required string CodeChallenge { get; set; }

        /// <summary>
        /// The method used to generate the code challenge. Should be "S256".
        /// </summary>
        public required string CodeChallengeMethod { get; set; }

        /// <summary>
        /// The user who authorized this code.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The application (client) that requested this code.
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// The redirect URI that was specified in the authorization request.
        /// Must match exactly when exchanging the code for tokens.
        /// </summary>
        public required string RedirectUri { get; set; }

        /// <summary>
        /// Optional OAuth2 scopes requested.
        /// </summary>
        public string? Scope { get; set; }

        /// <summary>
        /// When this authorization code expires (typically 60 seconds).
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Whether this code has been used. Codes can only be used once.
        /// </summary>
        public bool IsUsed { get; set; } = false;

        /// <summary>
        /// When this authorization code was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Application Application { get; set; } = null!;
    }
}
