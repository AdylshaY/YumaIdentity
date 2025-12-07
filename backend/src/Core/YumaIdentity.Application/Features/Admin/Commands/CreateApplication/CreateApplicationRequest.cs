namespace YumaIdentity.Application.Features.Admin.Commands.CreateApplication
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using System.ComponentModel.DataAnnotations;

    public class CreateApplicationRequest : IRequest<CreateApplicationResponse>
    {
        [Required(ErrorMessage = "AppName is required.")]
        [MinLength(3, ErrorMessage = "AppName must be at least 3 characters long.")]
        public required string AppName { get; set; }
        public string? AllowedCallbackUrls { get; set; }
        public string? ClientBaseUrl { get; set; }
        public bool IsIsolated { get; set; } = false;
    }
}
