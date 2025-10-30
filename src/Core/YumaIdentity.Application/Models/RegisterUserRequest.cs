namespace YumaIdentity.Application.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterUserRequest
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
