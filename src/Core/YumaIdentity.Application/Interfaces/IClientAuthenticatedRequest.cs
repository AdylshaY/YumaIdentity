namespace YumaIdentity.Application.Interfaces
{
    public interface IClientAuthenticatedRequest
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
