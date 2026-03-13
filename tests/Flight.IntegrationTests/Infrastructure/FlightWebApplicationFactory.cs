using Flight.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Flight.IntegrationTests.Infrastructure;

/// <summary>
/// Factory permettant de démarrer l'application ASP.NET Core pour les tests d'intégration.
/// </summary>
public class FlightWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<FlightContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<FlightContext>(options =>
            {
                options.UseInMemoryDatabase("FlightNetTestDb_" + Guid.NewGuid());
            });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FlightContext>();

            db.Database.EnsureCreated();
        });
    }
}