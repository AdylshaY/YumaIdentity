namespace YumaIdentity.Domain.Entities
{
    using System;

    public class UserToken
    {
        public long Id { get; set; }
        public required string TokenHash { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }

        public Guid UserId { get; set; }
        public int TokenTypeId { get; set; }

        public virtual User User { get; set; }
        public virtual TokenType TokenType { get; set; }
    }
}
