using System.Reflection;
using Chaski.Application.Services.Auth;
using Chaski.Application.Services.Users;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Chaski.Infrastructure.DependencyInjection;

public static class ChaskiApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddScoped<AuthService>();

        // services.AddValidatorsFromAssembly(Assembly.Load("Chaski.Application"));
        //
        // ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) => memberInfo?.Name;

        return services;
    }
}