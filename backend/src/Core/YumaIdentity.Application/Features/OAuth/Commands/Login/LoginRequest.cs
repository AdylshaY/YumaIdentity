namespace YumaIdentity.Application.Features.OAuth.Commands.Login
{
    using System.ComponentModel.DataAnnotations;
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    /// <summary>
    /// Internal login request for OAuth UI.
    /// Creates an authentication session for the OAuth authorization flow.
    /// </summary>
    public class LoginRequest : IRequest<LoginResponse>
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
