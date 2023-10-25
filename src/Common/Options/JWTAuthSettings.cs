namespace Common.Options
{
    public class JwtAuthSettings
    {
        public required string ValidIssuer { get; set; }
        public required string Secret { get; set; }
        public int ExpiresInSec { get; set; } = 5;
    }
}
