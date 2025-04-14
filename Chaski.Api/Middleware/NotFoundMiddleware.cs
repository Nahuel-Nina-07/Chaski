using System.Net;
using System.Text.Json;
using Chaski.Application.Common;
using Chaski.Application.Common.Helpers;

namespace Chaski.Api.Middleware;

public class NotFoundMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<NotFoundMiddleware> _logger;

    public NotFoundMiddleware(RequestDelegate next, ILogger<NotFoundMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.NotFound && !context.Response.HasStarted)
        {
            _logger.LogWarning("Ruta no encontrada: {Path}", context.Request.Path);

            context.Response.ContentType = "application/json";

            var result = Result<object>.Failure(
                new List<string> { "La ruta solicitada no fue encontrada." },
                HttpStatusCode.NotFound
            );

            var jsonResponse = JsonSerializer.Serialize(result);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}