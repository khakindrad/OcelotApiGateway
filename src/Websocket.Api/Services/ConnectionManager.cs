using Websocket.Api.Interfaces;

namespace Websocket.Api.Services
{
    public class ConnectionManager : IConnectionManager
    {
        public async Task HandleConnection(IConnection connection)
        {
            await connection.KeepReceiving();
            await connection.Close();
        }
    }
}