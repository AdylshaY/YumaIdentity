namespace YumaIdentity.Application.Models
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;

    public class RefreshTokenRequest : IRequest<TokenResponse>
    {
        [Required]
        public required string RefreshToken { get; set; }

        [Required]
        public required string ClientId { get; set; }
    }
}
