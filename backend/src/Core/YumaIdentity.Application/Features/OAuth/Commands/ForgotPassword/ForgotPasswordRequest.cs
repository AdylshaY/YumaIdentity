namespace YumaIdentity.Application.Features.OAuth.Commands.ForgotPassword
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    /// <summary>
    /// Request to initiate a password reset flow.
    /// Sends a password reset email to the specified address.
    /// </summary>
    public class ForgotPasswordRequest : IRequest<Unit>
    {
        /// <summary>
        /// The email address of the user requesting password reset.
        /// </summary>
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public required string Email { get; set; }

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
