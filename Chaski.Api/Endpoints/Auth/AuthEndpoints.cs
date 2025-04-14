using Chaski.Application.Dtos;
using Chaski.Application.Dtos.Auth;
using Chaski.Application.Dtos.Users;
using Chaski.Application.Services.Auth;
using Chaski.Application.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Chaski.Api.Endpoints.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/login", async (LoginDto dto, AuthService service) =>
        {
            var result = await service.LoginAsync(dto);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        });
        group.MapPost("/register", async (CreateUserDto dto, UserService service) =>
        {
            var result = await service.CreateUserAsync(dto);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        });
        group.MapGet("/confirm-email", async (
            [FromQuery] string token, 
            [FromQuery] string email, 
            UserService service) =>
        {
            var result = await service.ConfirmEmailAsync(token, email);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        });
    }
}