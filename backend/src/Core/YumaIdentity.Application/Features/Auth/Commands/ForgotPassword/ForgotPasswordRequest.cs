using MediatR;
using System.ComponentModel.DataAnnotations;

namespace YumaIdentity.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordRequest : IRequest<Unit>
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
