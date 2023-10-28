using Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Common.Middlewares;

public sealed class ExceptionHandlerMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            await _next(httpContext).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            //Generate an error id
            var errorId = Guid.NewGuid();

            //Log this exception
            _logger.LogException($"{errorId}:{ex.Message}", ex);

            //Return custom error exception to client
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var error = new
            {
                Id = errorId,
                ErroMessage = $"Something went wrong! We are looking into resoliving this {errorId}."
            };

            await httpContext.Response.WriteAsJsonAsync(error).ConfigureAwait(false);
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}
