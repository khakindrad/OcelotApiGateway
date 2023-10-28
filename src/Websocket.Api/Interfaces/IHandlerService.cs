namespace Websocket.Api.Interfaces;

public interface IHandlerService
{
    Task<bool> SendMessage(byte[] bytes, int offset, int length);
}