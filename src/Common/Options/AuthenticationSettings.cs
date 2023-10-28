namespace Common.Options;

public sealed class AuthenticationSettings
{
    public required AuthProvider Provider { get; set; }
    public required string ValidAudience { get; set; }
    public JwtAuthSettings? JWT { get; set; }
    public AwsCognitoAuthSettings? AWSCognito { get; set; }
}
