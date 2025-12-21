namespace YumaIdentity.Infrastructure.Options
{
    /// <summary>
    /// CORS configuration settings.
    /// </summary>
    public class CorsSettings
    {
        public const string SectionName = "Cors";

        /// <summary>
        /// Trusted origins for first-party applications (OAuth UI, Admin Portal, etc.).
        /// These are always allowed regardless of database configuration.
        /// </summary>
        public string[] TrustedOrigins { get; set; } = Array.Empty<string>();
    }
}
