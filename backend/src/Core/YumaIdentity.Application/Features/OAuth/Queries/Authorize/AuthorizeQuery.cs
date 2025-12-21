namespace YumaIdentity.Application.Features.OAuth.Queries.Authorize
{
    using System.ComponentModel.DataAnnotations;
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    /// <summary>
    /// OAuth2 authorization request query parameters.
    /// Initiates the authorization code flow.
    /// </summary>
    public class AuthorizeQuery : IRequest<AuthorizeQueryResponse>
    {
        [Required]
        public required string ClientId { get; set; }

        [Required]
        public required string RedirectUri { get; set; }

        [Required]
        public required string CodeChallenge { get; set; }

        [Required]
        public required string CodeChallengeMethod { get; set; }

        public string? State { get; set; }

        public string? Scope { get; set; }

        public string? SessionId { get; set; }
    }
}
