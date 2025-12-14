namespace YumaIdentity.Application.Features.OAuth.Commands.ResetPassword
{
    using System.ComponentModel.DataAnnotations;
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    /// <summary>
    /// Request to reset a user's password using a reset token.
    /// </summary>
    public class ResetPasswordRequest : IRequest<Unit>
    {
        /// <summary>
        /// The password reset token (composite format: UserId:RawToken).
        /// </summary>
        [Required]
        public required string Token { get; set; }

        /// <summary>
        /// The new password for the user account (minimum 8 characters).
        /// </summary>
        [Required]
        [MinLength(8)]
        public required string NewPassword { get; set; }
    }
}
