namespace YumaIdentity.Infrastructure.Options
{
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        public required string Key { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
}
