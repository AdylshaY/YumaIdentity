namespace YumaIdentity.Application.Features.OAuth.Queries.Authorize
{
    /// <summary>
    /// Response indicating what action should be taken for the authorization request.
    /// </summary>
    public class AuthorizeQueryResponse
    {
        public bool RequiresAuthentication { get; set; }
        public bool RequiresConsent { get; set; }
        public string? AuthorizationCode { get; set; }
        public string? State { get; set; }
        public int ExpiresIn { get; set; }
    }
}
