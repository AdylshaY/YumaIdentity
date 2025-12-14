namespace YumaIdentity.Application.Features.Admin.Queries.GetApplications
{
    using System;

    /// <summary>
    /// Response containing application details.
    /// </summary>
    public class ApplicationResponse
    {
        public Guid Id { get; set; }
        public required string AppName { get; set; }
        public required string ClientId { get; set; }
        public string? AllowedRedirectUris { get; set; }
    }
}
