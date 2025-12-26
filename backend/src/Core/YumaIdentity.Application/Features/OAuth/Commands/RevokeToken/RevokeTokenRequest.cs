namespace YumaIdentity.Application.Features.OAuth.Commands.RevokeToken
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
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
        [JsonPropertyName("token")]
        public required string Token { get; set; }

        /// <summary>
        /// Hint about the type of token being revoked.
        /// Values: "access_token" or "refresh_token"
        /// </summary>
        [JsonPropertyName("token_type_hint")]
        public string? TokenTypeHint { get; set; }

        /// <summary>
        /// The client ID of the application.
        /// </summary>
        [Required]
        [JsonPropertyName("client_id")]
        public required string ClientId { get; set; }
    }
}
