namespace YumaIdentity.Application.Features.OAuth.Commands.RegisterUser
{
    using System.ComponentModel.DataAnnotations;
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
        public required string Email { get; set; }

        /// <summary>
        /// The password for the new user account (minimum 8 characters).
        /// </summary>
        [Required]
        [MinLength(8)]
        public required string Password { get; set; }

        /// <summary>
        /// Optional client application identifier.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Optional client secret (for confidential clients).
        /// </summary>
        public string? ClientSecret { get; set; }
    }
}
