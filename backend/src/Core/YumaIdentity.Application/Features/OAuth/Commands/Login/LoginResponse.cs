namespace YumaIdentity.Application.Features.OAuth.Commands.Login
{
    /// <summary>
    /// Response for successful login containing session information.
    /// </summary>
    public class LoginResponse
    {
        public required string SessionId { get; set; }
        public required string Email { get; set; }
    }
}
