namespace YumaIdentity.Application.Features.Admin.Commands.CreateApplication
{
    using System;

    /// <summary>
    /// Response containing created application details.
    /// </summary>
    public class CreateApplicationResponse
    {
        public Guid Id { get; set; }
        public required string AppName { get; set; }
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
        public bool IsIsolated { get; set; }
        public string? AllowedRedirectUris { get; set; }
    }
}
