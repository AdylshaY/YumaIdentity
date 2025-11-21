namespace YumaIdentity.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class User
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? TenantId { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = [];

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];

        public virtual ICollection<UserToken> UserTokens { get; set; } = [];
    }
}
