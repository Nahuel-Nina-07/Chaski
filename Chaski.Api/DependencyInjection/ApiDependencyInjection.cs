using Microsoft.OpenApi.Models;

namespace Chaski.Api.DependencyInjection;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CHASKI API",
                Version = "v1"
            });
            c.EnableAnnotations();
        });

        services.AddCors(options =>
        {
            options.AddPolicy("CORSPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }
}