namespace YumaIdentity.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Features.OAuth.Commands.Authorize;
    using YumaIdentity.Application.Features.OAuth.Commands.ForgotPassword;
    using YumaIdentity.Application.Features.OAuth.Commands.RegisterUser;
    using YumaIdentity.Application.Features.OAuth.Commands.ResetPassword;
    using YumaIdentity.Application.Features.OAuth.Commands.Token;
    using YumaIdentity.Application.Features.OAuth.Commands.VerifyEmail;
    using YumaIdentity.Application.Features.OAuth.Commands.RevokeToken;
    using YumaIdentity.Application.Features.OAuth.Queries.UserInfo;
    using YumaIdentity.Application.Features.OAuth.Shared;

    /// <summary>
    /// OAuth2 endpoints for authentication and authorization.
    /// Implements the Authorization Code flow with PKCE for public clients.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OAuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OAuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var userId = await _mediator.Send(request);
            return Ok(new { UserId = userId });
        }

        /// <summary>
        /// Authorize a user and get an authorization code.
        /// This is the first step of the OAuth2 PKCE flow.
        /// </summary>
        [HttpPost("authorize")]
        [ProducesResponseType(typeof(AuthorizeResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Authorize([FromBody] AuthorizeRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Exchange an authorization code or refresh token for access tokens.
        /// </summary>
        [HttpPost("token")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Token([FromBody] TokenRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Get the current user's information.
        /// Requires a valid access token.
        /// </summary>
        [HttpGet("userinfo")]
        [Authorize]
        [ProducesResponseType(typeof(UserInfoResponse), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UserInfo()
        {
            var response = await _mediator.Send(new UserInfoQuery());
            return Ok(response);
        }

        /// <summary>
        /// Verify a user's email address.
        /// </summary>
        [HttpPost("verify-email")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            await _mediator.Send(request);
            return Ok(new { Message = "Email successfully verified." });
        }

        /// <summary>
        /// Request a password reset email.
        /// </summary>
        [HttpPost("forgot-password")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _mediator.Send(request);
            return Ok(new { Message = "Password reset link has been sent to your email." });
        }

        /// <summary>
        /// Reset a user's password using a reset token.
        /// </summary>
        [HttpPost("reset-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            await _mediator.Send(request);
            return Ok(new { Message = "Password has been reset successfully." });
        }

        /// <summary>
        /// Revoke an access or refresh token.
        /// Used for logout functionality.
        /// </summary>
        [HttpPost("revoke")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Revoke([FromBody] RevokeTokenRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
