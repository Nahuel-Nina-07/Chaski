using Chaski.Application.Services.Email;
using Chaski.Domain.Repositories.Users;
using Chaski.Domain.Security;
using Chaski.Infrastructure.Database.Context;
using Chaski.Infrastructure.Database.Repositories.Users;
using Chaski.Infrastructure.Security;
using Chaski.Infrastructure.Services;
using Chaski.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chaski.Infrastructure.DependencyInjection;

public static class ChaskiInfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("remoteConnection")!;

        services.AddDbContext<ChaskiDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        // Repositorios
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddTransient<IUserRoleRepository, UserRoleRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
        
        // Servicios de seguridad
        services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        
        
        // Servicio de email
        services.AddTransient<IEmailService, EmailService>();
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        
        // Configuración JWT
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        
        // HttpContextAccessor para obtener IPs
        services.AddHttpContextAccessor();

        return services;
    }
}