namespace YumaIdentity.Application.Features.OAuth.Commands.Authorize
{
    using System.ComponentModel.DataAnnotations;
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    /// <summary>
    /// Request to authorize a user and get an authorization code.
    /// This is the first step of the OAuth2 PKCE flow.
    /// </summary>
    public class AuthorizeRequest : IRequest<AuthorizeResponse>
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        /// <summary>
        /// The client application's identifier.
        /// </summary>
        [Required]
        public required string ClientId { get; set; }

        /// <summary>
        /// The URI to redirect to after authorization.
        /// Must match one of the application's AllowedRedirectUris.
        /// </summary>
        [Required]
        public required string RedirectUri { get; set; }

        /// <summary>
        /// PKCE code challenge (SHA256 hash of code_verifier, base64url encoded).
        /// Required for public clients (RFC 7636).
        /// </summary>
        [Required]
        public required string CodeChallenge { get; set; }

        /// <summary>
        /// The method used to generate the code challenge.
        /// Only "S256" is supported. "plain" is rejected for security reasons.
        /// Required per RFC 7636 - must be explicitly provided.
        /// </summary>
        [Required]
        public required string CodeChallengeMethod { get; set; }

        /// <summary>
        /// Optional OAuth2 scopes (space-separated).
        /// </summary>
        public string? Scope { get; set; }
    }
}

