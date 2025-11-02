namespace YumaIdentity.Application.Features.Auth.Commands.LoginUser
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    using YumaIdentity.Application.Features.Auth.Shared;
    using YumaIdentity.Application.Interfaces;

    public class LoginRequest : IRequest<TokenResponse>, IClientAuthenticatedRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [JsonIgnore]
        public string? ClientId { get; set; }

        [JsonIgnore]
        public string? ClientSecret { get; set; }
    }
}
