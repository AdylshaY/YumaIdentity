namespace YumaIdentity.Application.Features.Auth.Commands.RegisterUser
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using System.ComponentModel.DataAnnotations;

    public class RegisterUserRequest : IRequest<Guid>
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }

        public string? ClientId { get; set; }

        public string? ClientSecret { get; set; }
    }
}
