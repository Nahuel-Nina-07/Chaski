namespace Chaski.Api.Middleware;

public class TokenExpirationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenExpirationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized && 
            context.Items["TokenExpired"] as bool? == true)
        {
            context.Response.Headers.Add("Token-Expired", "true");
        }
    }
}