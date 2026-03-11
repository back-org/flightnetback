using Flight.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Flight.IntegrationTests;

/// <summary>
/// Factory WebApplication pour les tests d'intégration.
/// Remplace la base de données réelle par une base SQLite en mémoire.
/// </summary>
public class FlightWebApplicationFactory : WebApplicationFactory<Flight.Api.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Supprimer le DbContext existant
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<FlightContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Ajouter un DbContext en mémoire pour les tests
            services.AddDbContext<FlightContext>(options =>
            {
                options.UseInMemoryDatabase("FlightNetTestDb_" + Guid.NewGuid());
            });

            // Initialiser la base de données de test
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FlightContext>();
            db.Database.EnsureCreated();
        });
    }
}
