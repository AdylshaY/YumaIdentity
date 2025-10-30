namespace YumaIdentity.Application.Models
{
    using System;

    public class TokenResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}
