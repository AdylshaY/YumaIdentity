namespace YumaIdentity.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a user in the identity system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User's email address (unique per tenant).
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// BCrypt hashed password.
        /// </summary>
        public required string HashedPassword { get; set; }

        /// <summary>
        /// Whether the user's email address has been verified.
        /// </summary>
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// When the user account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// When the user account was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Tenant ID for multi-tenancy. Null for global users.
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// User's role assignments.
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];

        /// <summary>
        /// User's refresh tokens.
        /// </summary>
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];

        /// <summary>
        /// User's verification/reset tokens.
        /// </summary>
        public virtual ICollection<UserToken> UserTokens { get; set; } = [];
    }
}
