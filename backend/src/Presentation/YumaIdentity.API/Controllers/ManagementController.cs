namespace YumaIdentity.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using YumaIdentity.API.Filters;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Features.Management.Commands.RemoveAppUsers;
    using YumaIdentity.Application.Features.Management.Queries.GetAppUsers;
    using YumaIdentity.Application.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [RequireTenant]
    public class ManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        private string CurrentClientId => _currentUserService.ClientId!;

        public ManagementController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetMyUsers()
        {
            var query = new GetAppUsersQuery(CurrentClientId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("users/{userId:guid}")]
        public async Task<IActionResult> RemoveUser(Guid userId)
        {
            var command = new RemoveAppUserCommand(userId, CurrentClientId);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}