namespace YumaIdentity.Application.Features.Admin.Queries.GetUserById
{
    using System;
    using System.Collections.Generic;

    public class GetUserByIdResponse
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<UserRoleMembershipDto> Roles { get; set; } = [];
    }

    public class UserRoleMembershipDto
    {
        public Guid RoleId { get; set; }
        public required string RoleName { get; set; }
        public Guid ApplicationId { get; set; }
        public required string ApplicationName { get; set; }
    }
}
