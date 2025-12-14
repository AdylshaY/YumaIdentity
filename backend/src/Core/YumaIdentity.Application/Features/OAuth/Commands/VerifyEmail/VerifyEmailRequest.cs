namespace YumaIdentity.Application.Features.OAuth.Commands.VerifyEmail
{
    using System.ComponentModel.DataAnnotations;
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    /// <summary>
    /// Request to verify a user's email address.
    /// </summary>
    public class VerifyEmailRequest : IRequest<Unit>
    {
        /// <summary>
        /// The email verification token.
        /// </summary>
        [Required]
        public required string Token { get; set; }
    }
}
