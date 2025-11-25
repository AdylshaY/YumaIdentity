using MediatR;
using System.ComponentModel.DataAnnotations;

namespace YumaIdentity.Application.Features.Auth.Commands.VerifyEmail
{
    public class VerifyEmailRequest : IRequest<Unit>
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Token { get; set; }
    }
}
