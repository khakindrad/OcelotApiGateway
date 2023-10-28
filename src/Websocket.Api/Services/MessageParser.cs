using System.Text;
using Websocket.Api.Extensions;
using Websocket.Api.Interfaces;

namespace Websocket.Api.Services;

public sealed class MessageParser : IParser
{
    private readonly ILogger<MessageParser> _logger;

    public MessageParser(ILogger<MessageParser> logger)
    {
        _logger = logger;
        _logger.LogMessage(LogLevel.Debug, $"Started Service {GetType().Name}");
    }

    public string ParseMessage(byte[] bytes, int offset, int length)
    {
        var message = Encoding.UTF8.GetString(bytes, offset, length);

        _logger.LogMessage(LogLevel.Debug, $"Parsed message {message}");

        return message;
    }

    public byte[] GetBytes(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        return bytes;
    }
}
