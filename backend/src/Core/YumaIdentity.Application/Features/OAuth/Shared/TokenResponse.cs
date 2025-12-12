namespace YumaIdentity.Application.Features.OAuth.Shared
{
    /// <summary>
    /// OAuth2 token response (RFC 6749).
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// The access token for API authorization.
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// The refresh token for obtaining new access tokens.
        /// </summary>
        public required string RefreshToken { get; set; }

        /// <summary>
        /// Token type (always "Bearer").
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// How long until the access token expires (in seconds).
        /// </summary>
        public int ExpiresIn { get; set; }
    }
}
