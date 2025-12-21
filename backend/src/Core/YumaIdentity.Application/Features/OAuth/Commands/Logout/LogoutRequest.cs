namespace YumaIdentity.Application.Features.OAuth.Commands.Logout
{
    using System.ComponentModel.DataAnnotations;
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    /// <summary>
    /// Request to logout and clear the OAuth session.
    /// </summary>
    public class LogoutRequest : IRequest<Unit>
    {
        [Required]
        public required string SessionId { get; set; }
    }
}
