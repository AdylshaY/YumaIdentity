namespace YumaIdentity.Application.Models
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;

    public class LoginRequest : IRequest<TokenResponse>
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string ClientId { get; set; }
    }
}
