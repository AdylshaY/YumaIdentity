namespace YumaIdentity.Application.Features.Admin.Queries.GetRolesByApplicationId
{
    public class GetAppRolesResponse
    {
        public Guid RoleId { get; set; }
        public required string RoleName { get; set; }
    }
}