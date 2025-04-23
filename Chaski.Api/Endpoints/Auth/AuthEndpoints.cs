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

        group.MapPost("/login", async (LoginDto dto, AuthService service) =>
        {
            var result = await service.LoginAsync(dto);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        });

        group.MapPost("/refresh-token", async (RefreshTokenRequestDto request, AuthService service) =>
        {
            var result = await service.RefreshTokenAsync(request);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        });

        group.MapPost("/revoke-token", async (RevokeTokenRequestDto request, AuthService service) =>
        {
            var result = await service.RevokeTokenAsync(request);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        });

        group.MapPost("/revoke-all-tokens/{userId}", async (int userId, AuthService service) =>
        {
            var result = await service.RevokeAllTokensAsync(userId);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        }).RequireAuthorization();

        group.MapGet("/active-tokens/{userId}", async (int userId, AuthService service) =>
        {
            var result = await service.GetActiveRefreshTokensAsync(userId);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        }).RequireAuthorization();

        group.MapGet("/validate-token", () => 
        {
            return Results.Ok(new { message = "Token válido" });
        }).RequireAuthorization();
        
        group.MapPost("/forgot-password", async (ForgotPasswordDto dto, AuthService service) =>
        {
            var result = await service.ForgotPasswordAsync(dto.Email);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        }).RequireRateLimiting("password-reset");

        group.MapPost("/reset-password", async (ResetPasswordDto dto, AuthService service) =>
        {
            var result = await service.ResetPasswordAsync(dto);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        }).RequireRateLimiting("password-reset");
    }
}