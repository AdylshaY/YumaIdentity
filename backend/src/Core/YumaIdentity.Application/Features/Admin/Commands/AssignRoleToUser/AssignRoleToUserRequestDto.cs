namespace YumaIdentity.Application.Features.Admin.Commands.AssignRoleToUser
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AssignRoleToUserRequestDto
    {
        [Required]
        public Guid RoleId { get; set; }
    }
}
