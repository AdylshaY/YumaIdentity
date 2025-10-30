namespace YumaIdentity.Domain.Entities
{
    using System;

    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public virtual required User User { get; set; }
        public virtual required AppRole Role { get; set; }
    }
}
