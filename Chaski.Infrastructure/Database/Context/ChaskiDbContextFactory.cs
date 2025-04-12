using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Chaski.Infrastructure.Database.Context;

public class ChaskiDbContextFactory: IDesignTimeDbContextFactory<ChaskiDbContext>
{
    public ChaskiDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true) // ✅ ahora es opcional
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddUserSecrets<ChaskiDbContextFactory>(optional: true) // ✅ busca en secretos
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("remoteConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("No se encontró la cadena de conexión 'remoteConnection'.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<ChaskiDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ChaskiDbContext(optionsBuilder.Options);
    }
}