using System.Net.WebSockets;

namespace Websocket.Api.Interfaces
{
    public interface IConnectionFactory
    {
        IConnection CreateConnection(WebSocket webSocket);
    }
}