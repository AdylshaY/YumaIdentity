namespace YumaIdentity.Infrastructure.Options
{
    /// <summary>
    /// JWT configuration settings.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Configuration section name.
        /// </summary>
        public const string SectionName = "Jwt";

        /// <summary>
        /// Secret key for signing tokens.
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Token issuer.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Token audience.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Access token expiration in minutes.
        /// </summary>
        public int AccessTokenExpirationMinutes { get; set; }

        /// <summary>
        /// Refresh token expiration in days.
        /// </summary>
        public int RefreshTokenExpirationDays { get; set; }
    }
}
