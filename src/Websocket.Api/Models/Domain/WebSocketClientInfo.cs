using System.Net.WebSockets;

namespace Websocket.Api.Models.Domain;

public sealed class WebSocketClientInfo
{
    public required string ClientId { get; set; }
    public required WebSocket Socket { get; set; }
}
