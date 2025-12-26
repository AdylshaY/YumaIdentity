namespace YumaIdentity.Application.Features.OAuth.Commands.RegisterUser
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    /// <summary>
    /// Request to register a new user.
    /// </summary>
    public class RegisterUserRequest : IRequest<Guid>
    {
        /// <summary>
        /// The email address for the new user account.
        /// </summary>
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public required string Email { get; set; }

        /// <summary>
        /// The password for the new user account (minimum 8 characters).
        /// </summary>
        [Required]
        [MinLength(8)]
        [JsonPropertyName("password")]
        public required string Password { get; set; }

        /// <summary>
        /// Optional client application identifier.
        /// </summary>
        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }

        /// <summary>
        /// Optional client secret (for confidential clients).
        /// </summary>
        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }
    }
}
