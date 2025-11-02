namespace YumaIdentity.Application.Features.Auth.Commands.RegisterUser
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    using YumaIdentity.Application.Interfaces;

    public class RegisterUserRequest : IRequest<Guid>, IClientAuthenticatedRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }

        [JsonIgnore]
        public string? ClientId { get; set; }

        [JsonIgnore]
        public string? ClientSecret { get; set; }
    }
}
