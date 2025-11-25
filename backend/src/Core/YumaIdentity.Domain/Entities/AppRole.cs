namespace YumaIdentity.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class AppRole
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public Guid ApplicationId { get; set; }

        public virtual Application Application { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
