namespace YumaIdentity.API.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using YumaIdentity.Application.Features.Auth.Commands.LoginUser;
    using YumaIdentity.Application.Features.Auth.Commands.RefreshToken;
    using YumaIdentity.Application.Features.Auth.Commands.RegisterUser;
    using YumaIdentity.API.Filters;

    [ApiController]
    [Route("api/[controller]")]
    [InjectClientDataFilter]
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
    }
}
