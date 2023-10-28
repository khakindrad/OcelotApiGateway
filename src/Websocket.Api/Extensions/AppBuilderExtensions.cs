using System.Net;
using Common.Middlewares;
using Websocket.Api.Interfaces;
using Websocket.Api.Options;

namespace Websocket.Api.Extensions;

public static class AppBuilderExtensions
{
    public static void UseCustomMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
    }

    public static void UseWebSocketServer(this IApplicationBuilder app, IConfiguration configuration, ILogger logger)
    {
        var webSocketConfig = configuration.GetSection(WebSocketConfig.Name).Get<WebSocketConfig>();

        //Using web sockets
        var webSocketOptions = new WebSocketOptions();

        if (webSocketConfig is not null)
        {
            webSocketOptions.KeepAliveInterval = TimeSpan.FromMilliseconds(webSocketConfig.KeepAliveTimeIntervaInMS);
        }

        app.UseWebSockets(webSocketOptions);

        var webHandlerService = app.ApplicationServices.GetService<IWebHandlerService>();

        if (webHandlerService is not null)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        await webHandlerService.ClientConnected(context).ConfigureAwait(false);
                        //WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        //await Echo(context, webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    await next().ConfigureAwait(false);
                }

            });
        }
        else
        {
            logger.LogMessage(LogLevel.Error, $"Websocket service is not configured.");
        }
    }

    public static void UseServerHandler(this IApplicationBuilder app, ILogger logger)
    {
        logger.LogMessage($"Starting Server handler service.");

        app.ApplicationServices.GetService<IServerHandlerService>();

        logger.LogMessage($"Started Server handler service.");
    }
}
