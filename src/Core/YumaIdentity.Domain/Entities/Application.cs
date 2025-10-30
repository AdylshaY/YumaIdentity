namespace YumaIdentity.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class Application
    {
        public Guid Id { get; set; }
        public required string AppName { get; set; }
        public required string ClientId { get; set; }
        public required string HashedClientSecret { get; set; }
        public string? AllowedCallbackUrls { get; set; }

        public virtual ICollection<AppRole> AppRoles { get; set; } = [];
    }
}
