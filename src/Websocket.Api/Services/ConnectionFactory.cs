using System.Net.WebSockets;
using Websocket.Api.Interfaces;

namespace Websocket.Api.Services
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IConnection CreateConnection(WebSocket webSocket)
        {
            return new WebSocketConnection(webSocket);
        }
    }
}