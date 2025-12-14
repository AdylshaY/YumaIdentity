namespace YumaIdentity.Domain.Entities
{
    using System;

    /// <summary>
    /// Represents a refresh token for obtaining new access tokens.
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The hashed refresh token value.
        /// </summary>
        public required string TokenHash { get; set; }

        /// <summary>
        /// When the token expires.
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// When the token was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Whether the token has been revoked.
        /// </summary>
        public bool IsRevoked { get; set; }

        /// <summary>
        /// When the token was revoked (null if not revoked).
        /// </summary>
        public DateTime? RevokedAt { get; set; }

        /// <summary>
        /// The application this token belongs to.
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// The user this token belongs to.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Navigation property to User.
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// Navigation property to Application.
        /// </summary>
        public virtual Application Application { get; set; } = null!;
    }
}
