using System.Net;
using System.Text.Json;
using Chaski.Application.Common;
using Chaski.Application.Common.Helpers;

namespace Chaski.Api.Middleware;

public class MiddlewareException
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MiddlewareException> _logger;

    public MiddlewareException(RequestDelegate next, ILogger<MiddlewareException> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception intercepted by middleware");

            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = MapStatusCode(ex);
            response.StatusCode = (int)statusCode;

            var result = CreateErrorResponse(ex, statusCode);

            var jsonResponse = JsonSerializer.Serialize(result);
            await response.WriteAsync(jsonResponse);
        }
    }

    private static HttpStatusCode MapStatusCode(Exception ex)
    {
        return HttpStatusCode.InternalServerError;
    }

    private static Result<object> CreateErrorResponse(Exception exception, HttpStatusCode statusCode)
    {
        var errors = new List<string>();
        CollectErrorMessages(exception, errors);

        return Result<object>.Failure(errors, statusCode);
    }

    private static void CollectErrorMessages(Exception? exception, List<string> errorList)
    {
        if (exception == null)
            return;

        if (exception is AggregateException aggEx)
        {
            foreach (var inner in aggEx.InnerExceptions)
                CollectErrorMessages(inner, errorList);
        }
        else
        {
            errorList.Add(exception.Message);
            CollectErrorMessages(exception.InnerException, errorList);
        }
    }
}