using Microsoft.AspNetCore.Http;

namespace Common.Middlewares;

public sealed class WebSocketSecurityMiddleware
{
    private readonly RequestDelegate _next;

    // stored access token usually retrieved from any storage
    // implemented thought OAuth or any other identity protocol
    //private const string access_token = "821e2f35-86e3-4917-a963-b0c4228d1315";

    public WebSocketSecurityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // check, if request is a TX Text Control WebSocket request
        if (context.WebSockets.IsWebSocketRequest/* && context.WebSockets.WebSocketRequestedProtocols.Contains("TXTextControl.Web")*/)
        {
            // retrieve access token from query string
            //var sAccess_token = context.Request.Query["access_token"];

            // show case only: easy comparison of tokens 
            //if (sAccess_token != access_token)
            //    throw new UnauthorizedAccessException();
            //else
            await _next.Invoke(context).ConfigureAwait(false);
        }
        else if (_next != null)
        {
            await _next.Invoke(context).ConfigureAwait(false);
        }
    }
}
