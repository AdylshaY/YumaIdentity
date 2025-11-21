namespace YumaIdentity.Application.Features.Auth.Commands.RefreshToken
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;
    using YumaIdentity.Application.Features.Auth.Shared;

    public class RefreshTokenRequest : IRequest<TokenResponse>
    {
        [Required]
        public required string RefreshToken { get; set; }

        public string? ClientId { get; set; }

        public string? ClientSecret { get; set; }
    }
}
