namespace YumaIdentity.Application.Interfaces
{
    using YumaIdentity.Domain.Entities;

    public interface IClientValidator
    {
        Task<Application> ValidateAndGetApplicationAsync(string clientId, string clientSecret);
    }
}
