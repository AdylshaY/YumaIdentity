namespace YumaIdentity.Domain.Entities
{
    using System;

    /// <summary>
    /// Join table linking users to roles (many-to-many).
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// The user's ID.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The role's ID.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Navigation property to User.
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// Navigation property to AppRole.
        /// </summary>
        public virtual AppRole Role { get; set; } = null!;
    }
}
