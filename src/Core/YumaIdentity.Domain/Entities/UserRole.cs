namespace YumaIdentity.Domain.Entities
{
    using System;

    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual AppRole Role { get; set; }
    }
}
