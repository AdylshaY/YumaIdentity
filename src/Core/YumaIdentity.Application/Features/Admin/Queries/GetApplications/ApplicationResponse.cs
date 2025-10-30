namespace YumaIdentity.Application.Features.Admin.Queries.GetApplications
{
    using System;

    public class ApplicationResponse
    {
        public Guid Id { get; set; }
        public required string AppName { get; set; }
        public required string ClientId { get; set; }
        public string? AllowedCallbackUrls { get; set; }
    }
}
