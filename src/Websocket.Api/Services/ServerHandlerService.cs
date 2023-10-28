using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using Websocket.Api.Extensions;
using Websocket.Api.Interfaces;
using Websocket.Api.Options;

namespace Websocket.Api.Services;

public sealed class ServerHandlerService : IServerHandlerService, IDisposable
{
    private readonly ILogger<ServerHandlerService> _logger;
    private readonly IServerMessageProcessor _messageProcessor;
    private readonly Socket _socket;
    private readonly IPEndPoint _tcpEndPoint;

    public ServerHandlerService(ILogger<ServerHandlerService> logger, IServerMessageProcessor messageProcessor, IOptions<ServerConfig> options)
    {
        _logger = logger;
        _messageProcessor = messageProcessor;

        var serverConfig = options.Value;

        _logger.LogMessage(LogLevel.Debug, $"Started Service {GetType().Name}");

        _logger.LogMessage(LogLevel.Debug, $"Starting TCP Server socket receiving.");

        if (IPAddress.TryParse(serverConfig.IPAddress, out var ipAddress))
        {
            _tcpEndPoint = new IPEndPoint(ipAddress, serverConfig.Port);

            _socket = new(_tcpEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _ = StartReceiving();
        }
        else
        {
            throw new ArgumentException($"Invalid parameter value for serverConfig.IPAddress {serverConfig.IPAddress}", nameof(options));
        }
    }

    private async Task StartReceiving()
    {
        _logger.LogMessage(LogLevel.Debug, $"Connecting to TCP Server socket {_tcpEndPoint}.");

        await _socket.ConnectAsync(_tcpEndPoint).ConfigureAwait(false);

        _logger.LogMessage(LogLevel.Debug, $"Connected to TCP Server socket {_tcpEndPoint}.");

        while (true)
        {
            // Receive ack.
            var buffer = new byte[1_024];
            var received = await _socket.ReceiveAsync(buffer, SocketFlags.None).ConfigureAwait(false);

            if (received == 0 && !_socket.Connected)
            {
                _logger.LogMessage(LogLevel.Debug, $"Disconnected from server {_tcpEndPoint} {received} {_socket.Connected}");
                break;
            }

            await _messageProcessor.ProcessMessage(buffer, 0, received).ConfigureAwait(false);
        }

        _socket.Shutdown(SocketShutdown.Both);
    }

    public async Task<bool> SendMessage(byte[] bytes, int offset, int length)
    {
        _logger.LogMessage(LogLevel.Debug, $"Msg Received : {bytes.Length}, {offset}, {length}");

        var result = await _socket.SendAsync(bytes, SocketFlags.None).ConfigureAwait(false);

        return result > 0;
    }

    public void Dispose()
    {
        _socket?.Dispose();
    }
}
