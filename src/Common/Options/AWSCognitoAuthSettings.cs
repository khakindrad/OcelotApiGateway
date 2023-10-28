namespace Common.Options;

public sealed class AwsCognitoAuthSettings
{
    public required string Authority { get; set; }
    public required string Region { get; set; }
    public required string UserPoolId { get; set; }
    public required string ClientId { get; set; }
    public required string AuthFlowType { get; set; }
}
