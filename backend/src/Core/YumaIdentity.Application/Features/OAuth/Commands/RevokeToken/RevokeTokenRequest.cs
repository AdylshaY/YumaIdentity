namespace YumaIdentity.Application.Features.OAuth.Commands.RevokeToken
{
    using System.ComponentModel.DataAnnotations;
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    /// <summary>
    /// Request to revoke an access or refresh token.
    /// Used for logout functionality.
    /// </summary>
    public class RevokeTokenRequest : IRequest<Unit>
    {
        /// <summary>
        /// The token to revoke.
        /// </summary>
        [Required]
        public required string Token { get; set; }

        /// <summary>
        /// Hint about the type of token being revoked.
        /// Values: "access_token" or "refresh_token"
        /// </summary>
        public string? TokenTypeHint { get; set; }

        /// <summary>
        /// The client ID of the application.
        /// </summary>
        [Required]
        public required string ClientId { get; set; }
    }
}
