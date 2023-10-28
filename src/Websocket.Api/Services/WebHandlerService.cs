using System.Net.WebSockets;
using Websocket.Api.Extensions;
using Websocket.Api.Interfaces;
using Websocket.Api.Models.Domain;

namespace Websocket.Api.Services;

public sealed class WebHandlerService : IWebHandlerService
{
    private readonly IList<WebSocketClientInfo> _clients;
    private readonly ILogger<WebHandlerService> _logger;
    private readonly IWebMessageProcessor _messageProcessor;

    public WebHandlerService(ILogger<WebHandlerService> logger, IWebMessageProcessor messageProcessor)
    {
        _logger = logger;
        _messageProcessor = messageProcessor;

        _clients = new List<WebSocketClientInfo>();
        _logger.LogMessage(LogLevel.Debug, $"Started Service {GetType().Name}");
    }

    public async Task ClientConnected(HttpContext context)
    {
        using var socket = await context.WebSockets.AcceptWebSocketAsync(subProtocol: null).ConfigureAwait(false);

        var socketId = context.Connection.Id;

        var connection = new WebSocketClientInfo()
        {
            ClientId = socketId,
            Socket = socket,
        };

        _clients.Add(connection);

        _logger.LogMessage($"WebSocket connection established with ID {socketId}");

        _logger.LogMessage($"Total {_clients.Count} users connected.");

        var closeStatus = await StartReceivingAsync(connection).ConfigureAwait(false);

        if (closeStatus is not null)
        {
            await RemoveSocket(connection).ConfigureAwait(false);
        }
    }

    private async Task<WebSocketCloseStatus?> StartReceivingAsync(WebSocketClientInfo clientInfo)
    {
        WebSocketReceiveResult message;
        do
        {
            using var memoryStream = new MemoryStream();
            message = await ReceiveAsync(clientInfo, memoryStream).ConfigureAwait(false);
            if (message.Count > 0)
            {
                await _messageProcessor.ProcessMessage(memoryStream.ToArray(), 0, message.Count).ConfigureAwait(false);
            }
        } while (message.MessageType != WebSocketMessageType.Close);

        return message.CloseStatus;
    }

    private static async Task<WebSocketReceiveResult> ReceiveAsync(WebSocketClientInfo clientInfo, Stream memoryStream)
    {
        var readBuffer = new ArraySegment<byte>(new byte[4 * 1024]);
        WebSocketReceiveResult result;
        do
        {
            result = await clientInfo.Socket.ReceiveAsync(readBuffer, CancellationToken.None).ConfigureAwait(false);

            await memoryStream.WriteAsync(readBuffer.Array.AsMemory(readBuffer.Offset, result.Count), CancellationToken.None).ConfigureAwait(false);

        } while (!result.EndOfMessage);

        return result;
    }

    private async Task<bool> RemoveSocket(WebSocketClientInfo connection)
    {
        if (!_clients.Contains(connection))
        {
            return false;
        }

        var flag = _clients.Remove(connection);

        await connection.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).ConfigureAwait(false);

        _logger.LogMessage($"WebSocket connection closed with ID {connection.ClientId}");

        _logger.LogMessage($"Total {_clients.Count} users connected.");

        return flag;
    }

    public async Task<bool> SendMessage(byte[] bytes, int offset, int length)
    {
        _logger.LogMessage(LogLevel.Debug, $"Msg Received : {bytes.Length}, {offset}, {length}");

        foreach (var connectionInfo in _clients)
        {
            if (connectionInfo.Socket.State == WebSocketState.Open)
            {
                await connectionInfo.Socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false);
            }
            else
            {
                _logger.LogMessage($"Unable to send message to {connectionInfo.ClientId} due to {connectionInfo.Socket.State}");
            }
        }

        return true;
    }
}
