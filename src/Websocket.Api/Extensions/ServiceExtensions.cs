using Websocket.Api.Interfaces;
using Websocket.Api.Options;
using Websocket.Api.Services;

namespace Websocket.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IParser, MessageParser>();
            services.AddSingleton<IWebMessageProcessor, WebMessageProcessor>();
            services.AddSingleton<IServerMessageProcessor, ServerMessageProcessor>();
            services.AddSingleton<IWebHandlerService, WebHandlerService>();
            services.AddSingleton<IServerHandlerService, ServerHandlerService>();            
        }

        public static void RegisterCustomOptions(this IServiceCollection services)
        {
            services.AddOptions<ServerConfig>().BindConfiguration(ServerConfig.Name).ValidateDataAnnotations().ValidateOnStart();
            services.AddOptions<WebSocketConfig>().BindConfiguration(WebSocketConfig.Name).ValidateDataAnnotations().ValidateOnStart();
        }
    }
}
