using System.Net.WebSockets;

namespace Websocket.Api.Interfaces
{
    public interface IConnection
    {
        Task<WebSocketCloseStatus?> KeepReceiving();
        Task Send(string message);
        Task Close();
    }
}