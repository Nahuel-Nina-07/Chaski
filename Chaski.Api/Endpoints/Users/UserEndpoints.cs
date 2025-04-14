using Chaski.Application.Dtos;
using Chaski.Application.Dtos.Users;
using Chaski.Application.Services.Users;

namespace Chaski.Api.Endpoints.Users;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users").WithTags("Users");
        group.RequireAuthorization(); 
        group.MapGet("", async (UserService service) =>
        {
            var result = await service.GetAllUsersAsync();
            return Results.Json(result, statusCode: (int)result.StatusCode);
        });

        group.MapGet("/{id:int}", async (int id, UserService service) =>
        {
            var result = await service.GetUserByIdAsync(id);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        });
        
        group.MapPut("", async (UserDto dto, UserService service) =>
        {
            var result = await service.UpdateUserAsync(dto);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        });
    }
}