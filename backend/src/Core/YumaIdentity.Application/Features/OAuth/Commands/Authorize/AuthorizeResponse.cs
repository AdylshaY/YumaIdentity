namespace YumaIdentity.Application.Features.OAuth.Commands.Authorize
{
    /// <summary>
    /// Response containing the authorization code.
    /// </summary>
    public class AuthorizeResponse
    {
        /// <summary>
        /// The authorization code to exchange for tokens.
        /// </summary>
        public required string AuthorizationCode { get; set; }

        /// <summary>
        /// How long until the authorization code expires (in seconds).
        /// </summary>
        public int ExpiresIn { get; set; } = 60;
    }
}
