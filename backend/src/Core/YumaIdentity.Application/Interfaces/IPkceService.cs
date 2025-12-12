namespace YumaIdentity.Application.Interfaces
{
    /// <summary>
    /// Service for PKCE (Proof Key for Code Exchange) operations.
    /// Used to validate OAuth2 authorization requests from public clients.
    /// </summary>
    public interface IPkceService
    {
        /// <summary>
        /// Validates that the code_verifier matches the code_challenge.
        /// Uses SHA256 hashing for S256 method.
        /// </summary>
        /// <param name="codeVerifier">The original random string from the client</param>
        /// <param name="codeChallenge">The SHA256 hash that was sent during authorization</param>
        /// <param name="method">The challenge method (must be "S256")</param>
        /// <returns>True if the verifier matches the challenge</returns>
        bool ValidateCodeChallenge(string codeVerifier, string codeChallenge, string method);

        /// <summary>
        /// Generates a cryptographically secure random authorization code.
        /// </summary>
        /// <returns>A base64url-encoded random string</returns>
        string GenerateAuthorizationCode();
    }
}
