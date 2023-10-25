namespace Common.Options
{
    public class JWTAuthSettings
    {
        public required string ValidIssuer { get; set; }
        public required string Secret { get; set; }
    }
}
