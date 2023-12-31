using Websocket.Api.Extensions;
using Websocket.Api.Interfaces;

namespace Websocket.Api.Services;

public sealed class WebMessageProcessor : IWebMessageProcessor
{
    private readonly ILogger<WebMessageProcessor> _logger;
    private readonly IParser _parser;
    private readonly IServerHandlerService _serverHandler;

    public WebMessageProcessor(ILogger<WebMessageProcessor> logger, IParser parser, IServerHandlerService serverHandler)
    {
        _logger = logger;
        _parser = parser;
        _serverHandler = serverHandler;

        _logger.LogMessage(LogLevel.Debug, $"Started Service {GetType().Name}");
    }

    public async Task ProcessMessage(byte[] bytes, int offset, int length)
    {
        var message = _parser.ParseMessage(bytes, offset, length);

        await _serverHandler.SendMessage(bytes, offset, length).ConfigureAwait(false);

        _logger.LogMessage(LogLevel.Debug, $"Message processed {message}");
    }
}
