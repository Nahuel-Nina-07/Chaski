using Chaski.Domain.Repositories.Users;
using Chaski.Domain.Security;

namespace Chaski.Api.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenService _tokenService;

    public JwtMiddleware(RequestDelegate next, ITokenService tokenService)
    {
        _next = next;
        _tokenService = tokenService;
    }
    
    public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            if (principal?.Identity?.Name != null)
            {
                var user = await userRepository.GetByUsernameAsync(principal.Identity.Name);

                if (user != null)
                {
                    context.User = principal;
                }
            }
            else
            {
                context.Items["TokenExpired"] = true;
            }
        }
        await _next(context);
    }
}

