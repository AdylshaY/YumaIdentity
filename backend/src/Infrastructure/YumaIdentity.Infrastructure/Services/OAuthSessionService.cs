namespace YumaIdentity.Infrastructure.Services
{
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using YumaIdentity.Application.Interfaces;

    /// <summary>
    /// In-memory implementation of OAuth session management.
    /// For production, consider using distributed cache (Redis) for scalability.
    /// </summary>
    public class OAuthSessionService : IOAuthSessionService
    {
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan SessionTimeout = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan AuthRequestTimeout = TimeSpan.FromMinutes(5);

        public OAuthSessionService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void CreateSession(Guid userId, string sessionId)
        {
            var cacheKey = GetSessionCacheKey(sessionId);
            _cache.Set(cacheKey, userId, SessionTimeout);
        }

        public Guid? GetUserIdFromSession(string sessionId)
        {
            var cacheKey = GetSessionCacheKey(sessionId);
            return _cache.TryGetValue<Guid>(cacheKey, out var userId) ? userId : null;
        }

        public void RemoveSession(string sessionId)
        {
            var cacheKey = GetSessionCacheKey(sessionId);
            _cache.Remove(cacheKey);
        }

        public void StoreAuthorizationRequest(
            string sessionId,
            string clientId,
            string redirectUri,
            string codeChallenge,
            string codeChallengeMethod,
            string? state,
            string? scope)
        {
            var cacheKey = GetAuthRequestCacheKey(sessionId);
            var request = new AuthorizationRequestData
            {
                ClientId = clientId,
                RedirectUri = redirectUri,
                CodeChallenge = codeChallenge,
                CodeChallengeMethod = codeChallengeMethod,
                State = state,
                Scope = scope
            };
            _cache.Set(cacheKey, request, AuthRequestTimeout);
        }

        public (string ClientId, string RedirectUri, string CodeChallenge, string CodeChallengeMethod, string? State, string? Scope)? GetAuthorizationRequest(string sessionId)
        {
            var cacheKey = GetAuthRequestCacheKey(sessionId);
            if (_cache.TryGetValue<AuthorizationRequestData>(cacheKey, out var request) && request != null)
            {
                return (request.ClientId, request.RedirectUri, request.CodeChallenge, request.CodeChallengeMethod, request.State, request.Scope);
            }
            return null;
        }

        public void RemoveAuthorizationRequest(string sessionId)
        {
            var cacheKey = GetAuthRequestCacheKey(sessionId);
            _cache.Remove(cacheKey);
        }

        private static string GetSessionCacheKey(string sessionId) => $"oauth_session:{sessionId}";
        private static string GetAuthRequestCacheKey(string sessionId) => $"oauth_authreq:{sessionId}";

        private class AuthorizationRequestData
        {
            public required string ClientId { get; set; }
            public required string RedirectUri { get; set; }
            public required string CodeChallenge { get; set; }
            public required string CodeChallengeMethod { get; set; }
            public string? State { get; set; }
            public string? Scope { get; set; }
        }
    }
}
