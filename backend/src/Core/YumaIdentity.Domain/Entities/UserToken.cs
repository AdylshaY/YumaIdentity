namespace YumaIdentity.Domain.Entities
{
    using System;

    /// <summary>
    /// Represents a user verification or reset token (email verification, password reset).
    /// </summary>
    public class UserToken
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The hashed token value.
        /// </summary>
        public required string TokenHash { get; set; }

        /// <summary>
        /// When the token expires.
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Whether the token has been used.
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// The user this token belongs to.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The type of token (email verification, password reset, etc.).
        /// </summary>
        public int TokenTypeId { get; set; }

        /// <summary>
        /// Navigation property to User.
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// Navigation property to TokenType.
        /// </summary>
        public virtual TokenType TokenType { get; set; } = null!;
    }
}
