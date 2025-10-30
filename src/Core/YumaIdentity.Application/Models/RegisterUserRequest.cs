namespace YumaIdentity.Application.Models
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;

    public class RegisterUserRequest : IRequest<Guid>
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }

        [Required]
        public required string ClientId { get; set; }
    }
}
