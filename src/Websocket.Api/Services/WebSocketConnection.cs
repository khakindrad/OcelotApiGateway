using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;
using System.Net.WebSockets;
using System.Text;
using Websocket.Api.Interfaces;

namespace Websocket.Api.Services
{
    public class WebSocketConnection : IConnection
    {
        //private readonly ILogger<WebSocketConnection> _logger;
        private readonly WebSocket _webSocket;

        public WebSocketConnection(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        public async Task<WebSocketCloseStatus?> KeepReceiving()
        {
            WebSocketReceiveResult message;
            do
            {
                using var memoryStream = new MemoryStream();
                message = await ReceiveMessage(memoryStream);
                if (message.Count > 0)
                {
                    var receivedMessage = Encoding.UTF8.GetString(memoryStream.ToArray());
                    Log.Logger.Information($"Received message:{receivedMessage}");

                    await Send($"EcoBack: {receivedMessage}");
                }
            } while (message.MessageType != WebSocketMessageType.Close);

            return message.CloseStatus;
        }

        private async Task<WebSocketReceiveResult> ReceiveMessage(Stream memoryStream)
        {
            var readBuffer = new ArraySegment<byte>(new byte[4 * 1024]);
            WebSocketReceiveResult result;
            do
            {
                result = await _webSocket.ReceiveAsync(readBuffer, CancellationToken.None);
                await memoryStream.WriteAsync(readBuffer.Array, readBuffer.Offset, result.Count, CancellationToken.None);
            } while (!result.EndOfMessage);

            return result;
        }

        public async Task Send(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task Close()
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        }
    }
}