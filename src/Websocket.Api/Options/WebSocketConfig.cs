namespace Websocket.Api.Options;

public sealed class WebSocketConfig
{
    public const string Name = "WebSocketConfig";
    public int KeepAliveTimeIntervaInMS { get; set; }
}
