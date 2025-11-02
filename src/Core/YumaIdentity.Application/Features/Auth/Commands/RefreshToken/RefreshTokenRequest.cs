namespace YumaIdentity.Application.Features.Auth.Commands.RefreshToken
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    using YumaIdentity.Application.Features.Auth.Shared;
    using YumaIdentity.Application.Interfaces;

    public class RefreshTokenRequest : IRequest<TokenResponse>, IClientAuthenticatedRequest
    {
        [Required]
        public required string RefreshToken { get; set; }

        [JsonIgnore]
        public string? ClientId { get; set; }

        [JsonIgnore]
        public string? ClientSecret { get; set; }
    }
}
