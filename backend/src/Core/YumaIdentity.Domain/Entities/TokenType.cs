namespace YumaIdentity.Domain.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines token types (email verification, password reset, etc.).
    /// </summary>
    public class TokenType
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the token type.
        /// </summary>
        public required string TypeName { get; set; }

        /// <summary>
        /// Description of the token type.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Tokens of this type.
        /// </summary>
        public virtual ICollection<UserToken> UserTokens { get; set; } = [];
    }
}
