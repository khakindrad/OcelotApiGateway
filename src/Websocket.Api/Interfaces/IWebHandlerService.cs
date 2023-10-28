namespace Websocket.Api.Interfaces
{
    public interface IWebHandlerService : IHandlerService
    {
        public Task ClientConnected(HttpContext context);
    }
}