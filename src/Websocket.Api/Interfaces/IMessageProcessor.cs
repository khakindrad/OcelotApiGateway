namespace Websocket.Api.Interfaces;

public interface IMessageProcessor
{
    Task ProcessMessage(byte[] bytes, int offset, int length);
}