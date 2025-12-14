namespace YumaIdentity.Infrastructure.Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using YumaIdentity.Application.Interfaces;

    /// <summary>
    /// Implementation of PKCE (Proof Key for Code Exchange) operations.
    /// </summary>
    public class PkceService : IPkceService
    {
        /// <inheritdoc />
        public bool ValidateCodeChallenge(string codeVerifier, string codeChallenge, string method)
        {
            // Only S256 method is supported (plain is insecure)
            if (method != "S256")
            {
                return false;
            }

            try
            {
                // Compute SHA256 hash of the code_verifier
                var hash = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));

                // Base64Url encode the hash
                var computedChallenge = Base64UrlEncode(hash);

                // Compare with the provided code_challenge
                return string.Equals(computedChallenge, codeChallenge, StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc />
        public string GenerateAuthorizationCode()
        {
            // Generate 32 bytes of cryptographically secure random data
            var bytes = new byte[32];
            RandomNumberGenerator.Fill(bytes);

            return Base64UrlEncode(bytes);
        }

        /// <summary>
        /// Encodes bytes to Base64Url format (RFC 4648).
        /// This is URL-safe base64 without padding.
        /// </summary>
        private static string Base64UrlEncode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')           // Remove padding
                .Replace('+', '-')      // Replace + with -
                .Replace('/', '_');     // Replace / with _
        }
    }
}
