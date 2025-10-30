namespace YumaIdentity.Application.Features.Admin.Commands.CreateApplication
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;

    public class CreateApplicationRequest : IRequest<CreateApplicationResponse>
    {
        [Required(ErrorMessage = "AppName is required.")]
        [MinLength(3, ErrorMessage = "AppName must be at least 3 characters long.")]
        public required string AppName { get; set; }

        public string? AllowedCallbackUrls { get; set; }
    }
}
