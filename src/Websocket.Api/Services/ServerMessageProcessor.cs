using Websocket.Api.Extensions;
using Websocket.Api.Interfaces;

namespace Websocket.Api.Services
{
    public sealed class ServerMessageProcessor : IServerMessageProcessor
    {
        private readonly ILogger<ServerMessageProcessor> _logger;
        private readonly IParser _parser;
        //private readonly IWebHandlerService _webHandlerService;

        public ServerMessageProcessor(ILogger<ServerMessageProcessor> logger, IParser parser/*, IWebHandlerService webHandlerService*/)
        {
            _logger = logger;
            _parser = parser;
            //_webHandlerService = webHandlerService;

            _logger.LogMessage(LogLevel.Debug, $"Started Service {GetType().Name}");
        }

        public Task ProcessMessage(byte[] bytes, int offset, int length)
        {
            var message = _parser.ParseMessage(bytes, offset, length);

            //_webHandlerService.SendMessage(bytes, offset, length);

            return Task.FromResult(message);
        }
    }
}