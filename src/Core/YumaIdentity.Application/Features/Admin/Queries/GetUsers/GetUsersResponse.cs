namespace YumaIdentity.Application.Features.Admin.Queries.GetUsers
{
    using System;

    public class GetUsersResponse
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
