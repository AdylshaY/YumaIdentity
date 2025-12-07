namespace YumaIdentity.API.Controllers
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using YumaIdentity.Application.Features.Admin.Commands.AssignRoleToUser;
    using YumaIdentity.Application.Features.Admin.Commands.CreateApplication;
    using YumaIdentity.Application.Features.Admin.Commands.DeleteApplication;
    using YumaIdentity.Application.Features.Admin.Commands.DeleteUser;
    using YumaIdentity.Application.Features.Admin.Commands.RemoveRoleFromUser;
    using YumaIdentity.Application.Features.Admin.Queries.GetApplications;
    using YumaIdentity.Application.Features.Admin.Queries.GetRolesByApplicationId;
    using YumaIdentity.Application.Features.Admin.Queries.GetUserById;
    using YumaIdentity.Application.Features.Admin.Queries.GetUsers;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("applications")]
        [ProducesResponseType(typeof(List<ApplicationResponse>), 200)]
        public async Task<IActionResult> GetApplications()
        {
            var query = new GetApplicationsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("applications")]
        [ProducesResponseType(typeof(CreateApplicationResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationRequest request)
        {
            var result = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetApplications), new { id = result.Id }, result);
        }

        [HttpDelete("applications/{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteApplication(Guid id)
        {
            var command = new DeleteApplicationCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("users")]
        [ProducesResponseType(typeof(List<GetUsersResponse>), 200)]
        public async Task<IActionResult> GetUsers()
        {
            var query = new GetUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("users/{id:guid}")]
        [ProducesResponseType(typeof(GetUserByIdResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("users/{userId:guid}/roles")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AssignRoleToUser(Guid userId, [FromBody] AssignRoleToUserRequestDto request)
        {
            var command = new AssignRoleToUserCommand
            {
                UserId = userId,
                RoleId = request.RoleId
            };

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("users/{userId:guid}/roles/{roleId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveRoleFromUser(Guid userId, Guid roleId)
        {
            var command = new RemoveRoleFromUserCommand(userId, roleId);
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("users/{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("applications/{applicationId:guid}/roles")]
        [ProducesResponseType(typeof(List<GetAppRolesResponse>), 200)]
        public async Task<IActionResult> GetRolesByApplication(Guid applicationId)
        {
            var query = new GetRolesByApplicationIdQuery(applicationId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
