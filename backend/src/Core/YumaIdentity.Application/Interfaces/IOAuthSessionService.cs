namespace YumaIdentity.Application.Interfaces
{
    /// <summary>
    /// Service for managing OAuth authentication sessions.
    /// Used to track authenticated users during the OAuth authorization flow.
    /// </summary>
    public interface IOAuthSessionService
    {
        /// <summary>
        /// Creates an authentication session for a user.
        /// </summary>
        /// <param name="userId">The ID of the authenticated user</param>
        /// <param name="sessionId">The unique session identifier</param>
        void CreateSession(Guid userId, string sessionId);

        /// <summary>
        /// Retrieves the user ID from an active session.
        /// </summary>
        /// <param name="sessionId">The session identifier</param>
        /// <returns>The user ID if session is valid, null otherwise</returns>
        Guid? GetUserIdFromSession(string sessionId);

        /// <summary>
        /// Removes an authentication session.
        /// </summary>
        /// <param name="sessionId">The session identifier to remove</param>
        void RemoveSession(string sessionId);

        /// <summary>
        /// Stores authorization request parameters during the OAuth flow.
        /// </summary>
        /// <param name="sessionId">The session identifier</param>
        /// <param name="clientId">The client application ID</param>
        /// <param name="redirectUri">The redirect URI</param>
        /// <param name="codeChallenge">The PKCE code challenge</param>
        /// <param name="codeChallengeMethod">The PKCE challenge method</param>
        /// <param name="state">The OAuth state parameter</param>
        /// <param name="scope">The requested scopes</param>
        void StoreAuthorizationRequest(
            string sessionId,
            string clientId,
            string redirectUri,
            string codeChallenge,
            string codeChallengeMethod,
            string? state,
            string? scope);

        /// <summary>
        /// Retrieves stored authorization request parameters.
        /// </summary>
        /// <param name="sessionId">The session identifier</param>
        /// <returns>Tuple containing the authorization parameters, or null if not found</returns>
        (string ClientId, string RedirectUri, string CodeChallenge, string CodeChallengeMethod, string? State, string? Scope)? GetAuthorizationRequest(string sessionId);

        /// <summary>
        /// Removes stored authorization request parameters.
        /// </summary>
        /// <param name="sessionId">The session identifier</param>
        void RemoveAuthorizationRequest(string sessionId);
    }
}
