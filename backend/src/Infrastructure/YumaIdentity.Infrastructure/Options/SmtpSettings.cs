namespace YumaIdentity.Infrastructure.Options
{
    public class SmtpSettings
    {
        public const string SectionName = "SmtpSettings";

        public required string Host { get; set; }
        public int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public required string SenderName { get; set; }
        public required string SenderEmail { get; set; }
    }
}
