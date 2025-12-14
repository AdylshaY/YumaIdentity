namespace YumaIdentity.Application.Features.OAuth.Queries.UserInfo
{
    using System.Collections.Generic;

    /// <summary>
    /// Response containing user information (OpenID Connect style).
    /// </summary>
    public class UserInfoResponse
    {
        /// <summary>
        /// Subject identifier (user ID).
        /// </summary>
        public required string Sub { get; set; }

        /// <summary>
        /// User's email address.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Whether the email has been verified.
        /// </summary>
        public bool EmailVerified { get; set; }

        /// <summary>
        /// User's roles for the current application.
        /// </summary>
        public List<string> Roles { get; set; } = new();
    }
}
