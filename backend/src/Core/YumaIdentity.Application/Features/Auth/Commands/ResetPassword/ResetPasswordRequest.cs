using YumaIdentity.Application.Common.Interfaces.Mediator;
using System.ComponentModel.DataAnnotations;

namespace YumaIdentity.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordRequest : IRequest<Unit>
    {
        [Required]
        public required string Token { get; set; } // Composite Token (UserId:RawToken)

        [Required]
        [MinLength(8)]
        public required string NewPassword { get; set; }
    }
}
