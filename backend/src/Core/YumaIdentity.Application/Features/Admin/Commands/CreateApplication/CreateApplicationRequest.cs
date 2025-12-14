namespace YumaIdentity.Application.Features.Admin.Commands.CreateApplication
{
    using System.ComponentModel.DataAnnotations;
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    /// <summary>
    /// Request to create a new application.
    /// </summary>
    public class CreateApplicationRequest : IRequest<CreateApplicationResponse>
    {
        /// <summary>
        /// Name of the application.
        /// </summary>
        [Required(ErrorMessage = "AppName is required.")]
        [MinLength(3, ErrorMessage = "AppName must be at least 3 characters long.")]
        public required string AppName { get; set; }

        /// <summary>
        /// Comma-separated list of allowed OAuth2 redirect URIs.
        /// </summary>
        public string? AllowedRedirectUris { get; set; }

        /// <summary>
        /// Base URL of the client application.
        /// </summary>
        public string? ClientBaseUrl { get; set; }

        /// <summary>
        /// If true, application has its own isolated user pool.
        /// </summary>
        public bool IsIsolated { get; set; } = false;
    }
}
