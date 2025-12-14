namespace YumaIdentity.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a role within an application.
    /// </summary>
    public class AppRole
    {
        /// <summary>
        /// Unique identifier for the role.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the role (e.g., "Admin", "User").
        /// </summary>
        public required string RoleName { get; set; }

        /// <summary>
        /// The application this role belongs to.
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// Navigation property to Application.
        /// </summary>
        public virtual Application Application { get; set; } = null!;

        /// <summary>
        /// Users assigned to this role.
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
