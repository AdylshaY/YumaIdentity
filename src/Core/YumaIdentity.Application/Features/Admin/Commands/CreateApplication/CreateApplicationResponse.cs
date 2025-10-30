namespace YumaIdentity.Application.Features.Admin.Commands.CreateApplication
{
    using System;

    public class CreateApplicationResponse
    {
        public Guid Id { get; set; }
        public required string AppName { get; set; }
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
        public string? AllowedCallbackUrls { get; set; }
    }
}
