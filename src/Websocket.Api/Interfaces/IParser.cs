namespace Websocket.Api.Interfaces
{
    public interface IParser
    {
        string ParseMessage(byte[] bytes, int offset, int length);

        byte[] GetBytes(string message);
    }
}