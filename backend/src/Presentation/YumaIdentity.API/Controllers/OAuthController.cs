namespace YumaIdentity.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Features.OAuth.Commands.ForgotPassword;
    using YumaIdentity.Application.Features.OAuth.Commands.Login;
    using YumaIdentity.Application.Features.OAuth.Commands.Logout;
    using YumaIdentity.Application.Features.OAuth.Commands.RegisterUser;
    using YumaIdentity.Application.Features.OAuth.Commands.ResetPassword;
    using YumaIdentity.Application.Features.OAuth.Commands.Token;
    using YumaIdentity.Application.Features.OAuth.Commands.VerifyEmail;
    using YumaIdentity.Application.Features.OAuth.Commands.RevokeToken;
    using YumaIdentity.Application.Features.OAuth.Queries.Authorize;
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
        /// OAuth2 authorization endpoint (Standard flow).
        /// Initiates the authorization code flow with PKCE.
        /// </summary>
        [HttpGet("authorize")]
        [ProducesResponseType(typeof(AuthorizeQueryResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Authorize(
            [FromQuery(Name = "client_id")] string clientId,
            [FromQuery(Name = "redirect_uri")] string redirectUri,
            [FromQuery(Name = "code_challenge")] string codeChallenge,
            [FromQuery(Name = "code_challenge_method")] string codeChallengeMethod,
            [FromQuery(Name = "state")] string? state = null,
            [FromQuery(Name = "scope")] string? scope = null,
            [FromQuery(Name = "session_id")] string? sessionId = null)
        {
            var query = new AuthorizeQuery
            {
                ClientId = clientId,
                RedirectUri = redirectUri,
                CodeChallenge = codeChallenge,
                CodeChallengeMethod = codeChallengeMethod,
                State = state,
                Scope = scope,
                SessionId = sessionId
            };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        /// <summary>
        /// Internal login endpoint for OAuth UI.
        /// Creates an authentication session.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
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
        /// Logout and clear OAuth session.
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            await _mediator.Send(request);
            return Ok(new { Message = "Logged out successfully." });
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
