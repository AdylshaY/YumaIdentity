namespace YumaIdentity.Application.Features.Auth.Commands.LoginUser
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;
    using YumaIdentity.Application.Features.Auth.Shared;

    public class LoginRequest : IRequest<TokenResponse>
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        public string? ClientId { get; set; }

        public string? ClientSecret { get; set; }
    }
}
