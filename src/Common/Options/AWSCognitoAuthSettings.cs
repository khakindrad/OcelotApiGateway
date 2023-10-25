namespace Common.Options
{
    public class AwsCognitoAuthSettings
    {
        public required string Authority { get; set; }
        public string Region { get; set; }
        public string UserPoolId { get; set; }
        public string ClientId { get; set; }        
        public string AuthFlowType { get; set; }
    }
}
