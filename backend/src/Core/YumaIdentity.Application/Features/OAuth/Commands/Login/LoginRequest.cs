namespace YumaIdentity.Application.Features.OAuth.Commands.Login
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    /// <summary>
    /// Internal login request for OAuth UI.
    /// Creates an authentication session for the OAuth authorization flow.
    /// </summary>
    public class LoginRequest : IRequest<LoginResponse>
    {
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public required string Password { get; set; }

        [Required]
        [JsonPropertyName("client_id")]
        public required string ClientId { get; set; }
    }
}
