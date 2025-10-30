namespace YumaIdentity.API.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using YumaIdentity.Application.Features.Admin.Commands.CreateApplication;
    using YumaIdentity.Application.Features.Admin.Queries.GetApplications;
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
    }
}
