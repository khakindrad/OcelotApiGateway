namespace Websocket.Api.Interfaces
{
    public interface IConnectionManager
    {
        Task HandleConnection(IConnection connection);
    }
}