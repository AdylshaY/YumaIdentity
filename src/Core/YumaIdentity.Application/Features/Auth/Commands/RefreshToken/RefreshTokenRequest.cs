namespace YumaIdentity.Application.Features.Auth.Commands.RefreshToken
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;
    using YumaIdentity.Application.Features.Auth.Shared;

    public class RefreshTokenRequest : IRequest<TokenResponse>
    {
        [Required]
        public required string RefreshToken { get; set; }

        [Required]
        public required string ClientId { get; set; }
    }
}
