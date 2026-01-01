namespace YumaIdentity.API.Models.Requests
{
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;

    public class AuthorizeRequestDto
    {
        [Required]
        [FromQuery(Name = "client_id")]
        public required string ClientId { get; set; }

        [Required]
        [FromQuery(Name = "redirect_uri")]
        public required string RedirectUri { get; set; }

        [Required]
        [FromQuery(Name = "code_challenge")]
        public required string CodeChallenge { get; set; }

        [Required]
        [FromQuery(Name = "code_challenge_method")]
        public required string CodeChallengeMethod { get; set; }

        [FromQuery(Name = "state")]
        public string? State { get; set; }

        [FromQuery(Name = "scope")]
        public string? Scope { get; set; }

        [FromQuery(Name = "session_id")]
        public string? SessionId { get; set; }
    }
}
