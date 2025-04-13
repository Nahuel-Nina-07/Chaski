using Chaski.Domain.Repositories.Users;
using Chaski.Domain.Security;
using Chaski.Infrastructure.Database.Context;
using Chaski.Infrastructure.Database.Repositories.Users;
using Chaski.Infrastructure.Security;
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
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
        return services;
    }
}