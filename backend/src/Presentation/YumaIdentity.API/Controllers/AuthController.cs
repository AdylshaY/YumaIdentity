namespace YumaIdentity.API.Controllers
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using Microsoft.AspNetCore.Mvc;
    using YumaIdentity.Application.Features.Auth.Commands.ForgotPassword;
    using YumaIdentity.Application.Features.Auth.Commands.LoginUser;
    using YumaIdentity.Application.Features.Auth.Commands.RefreshToken;
    using YumaIdentity.Application.Features.Auth.Commands.RegisterUser;
    using YumaIdentity.Application.Features.Auth.Commands.ResetPassword;
    using YumaIdentity.Application.Features.Auth.Commands.VerifyEmail;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var userId = await _mediator.Send(request);
            return Ok(new { UserId = userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var tokenResponse = await _mediator.Send(request);
            return Ok(tokenResponse);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var tokenResponse = await _mediator.Send(request);
            return Ok(tokenResponse);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            await _mediator.Send(request);
            return Ok(new { Message = "Email successfully verified." });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _mediator.Send(request);
            return Ok(new { Message = "Password reset link has been sent to your email." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            await _mediator.Send(request);
            return Ok(new { Message = "Password has been reset successfully. You can now login with your new password." });
        }
    }
}
