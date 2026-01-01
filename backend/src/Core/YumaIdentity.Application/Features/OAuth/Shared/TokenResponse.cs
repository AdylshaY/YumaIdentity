namespace YumaIdentity.Application.Features.OAuth.Shared
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// OAuth2 token response (RFC 6749).
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// The access token for API authorization.
        /// </summary>
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }

        /// <summary>
        /// The refresh token for obtaining new access tokens.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public required string RefreshToken { get; set; }

        /// <summary>
        /// Token type (always "Bearer").
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// How long until the access token expires (in seconds).
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
