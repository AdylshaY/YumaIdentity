namespace YumaIdentity.Application.Features.Management.Queries.GetAppUsers
{
    public class GetAppUsersResponse
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
