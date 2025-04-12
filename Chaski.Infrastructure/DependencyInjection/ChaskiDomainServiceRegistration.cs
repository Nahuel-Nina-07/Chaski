using Microsoft.Extensions.DependencyInjection;

namespace Chaski.Infrastructure.DependencyInjection;

public static class ChaskiDomainServiceRegistration
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // Aquí irían servicios puramente del dominio si se tienen.
        // Ejemplo: services.AddTransient<IEncryptorService, EncryptorService>();

        return services;
    }
}