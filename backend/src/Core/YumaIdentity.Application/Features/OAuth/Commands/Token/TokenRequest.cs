namespace YumaIdentity.Application.Features.OAuth.Commands.Token
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Features.OAuth.Shared;

    /// <summary>
    /// Request to exchange an authorization code or refresh token for access tokens.
    /// </summary>
    public class TokenRequest : IRequest<TokenResponse>
    {
        /// <summary>
        /// The grant type. Supported values: "authorization_code", "refresh_token".
        /// </summary>
        [Required]
        [JsonPropertyName("grant_type")]
        public required string GrantType { get; set; }

        /// <summary>
        /// The client application's identifier.
        /// </summary>
        [Required]
        [JsonPropertyName("client_id")]
        public required string ClientId { get; set; }

        /// <summary>
        /// Client secret (required for Confidential clients, ignored for Public clients).
        /// </summary>
        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }

        /// <summary>
        /// The authorization code (required when grant_type is "authorization_code").
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>
        /// The PKCE code verifier (required for Public clients with authorization_code grant).
        /// </summary>
        [JsonPropertyName("code_verifier")]
        public string? CodeVerifier { get; set; }

        /// <summary>
        /// The redirect URI (must match the one used in the authorization request).
        /// </summary>
        [JsonPropertyName("redirect_uri")]
        public string? RedirectUri { get; set; }

        /// <summary>
        /// The refresh token (required when grant_type is "refresh_token").
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}

