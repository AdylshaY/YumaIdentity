namespace YumaIdentity.Domain.Entities
{
    using System.Collections.Generic;

    public class TokenType
    {
        public int Id { get; set; }
        public required string TypeName { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<UserToken> UserTokens { get; set; } = [];
    }
}
