namespace YumaIdentity.Domain.Entities
{
    using System;

    public class RefreshToken
    {
        public long Id { get; set; }
        public required string TokenHash { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRevoked { get; set; }

        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
        public virtual Application Application { get; set; }
    }
}
