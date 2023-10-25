namespace Common.Options
{
    public class AuthenticationSettings
    {
        public required AuthProvider Provider { get; set; }
        public required string ValidAudience { get; set; }
        public JWTAuthSettings? JWT { get; set; }
        public AWSCognitoAuthSettings? AWSCognito {  get; set; }        
    }
}
