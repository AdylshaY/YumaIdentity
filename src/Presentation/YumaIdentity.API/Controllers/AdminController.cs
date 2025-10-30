namespace YumaIdentity.API.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using YumaIdentity.Application.Features.Admin.Commands.CreateApplication;
    using YumaIdentity.Application.Features.Admin.Queries.GetApplications;

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
    }
}
