using Chaski.Api.DependencyInjection;
using Chaski.Api.Endpoints.Users;
using Chaski.Api.Middleware;
using Chaski.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables();

builder.Services
    .AddApiDependencies()
    .AddApplicationServices()
    .AddDomainServices()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseCors("CORSPolicy");
app.UseMiddleware<MiddlewareException>();
app.UseMiddleware<NotFoundMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CHASKI API v1");
    c.RoutePrefix = "swagger";
    c.EnableFilter();
});

app.MapUserEndpoints();

app.Run();
