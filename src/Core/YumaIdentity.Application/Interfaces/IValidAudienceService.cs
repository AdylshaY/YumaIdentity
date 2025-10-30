namespace YumaIdentity.Application.Interfaces
{
    public interface IValidAudienceService
    {
        Task<bool> IsAudienceValidAsync(IEnumerable<string> audiences);
    }
}
