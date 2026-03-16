using Asp.Versioning;
using DotNetEnv;
using FluentValidation;
using MediatR;
using FluentValidation.AspNetCore;
using Flight.Application.Applications;
using Flight.Application.Validators;
using Flight.Infrastructure.Auth;
using Flight.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Scalar.AspNetCore;

// Charge les variables définies dans le fichier .env.
// Evite un crash si le fichier n'existe pas
// dans certains environnements comme la CI/CD ou certains conteneurs.
try
{
    if (File.Exists(".env"))
    {
        Env.Load(".env");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Impossible de charger le fichier .env : {ex.Message}");
}

var builder = WebApplication.CreateBuilder(args);

// Ajoute les variables d'environnement système à la configuration.
builder.Configuration.AddEnvironmentVariables();

// ============================================================
// Enregistrement des services
// ============================================================

// Enregistre le DbContext et la connexion à la base de données.
builder.Services.AddDataContext(builder.Configuration);

// Configure CORS.
// En développement, cette politique ouverte facilite les tests front/API.
// En production, il est recommandé de limiter les origines autorisées.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Enregistre les contrôleurs MVC avec configuration JSON.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        // Sérialise les propriétés en camelCase avec Newtonsoft.Json.
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    })
    .AddJsonOptions(options =>
    {
        // Conserve les noms de propriétés tels qu'ils sont définis côté C#.
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Enregistre FluentValidation pour la validation automatique des DTO.
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<FlightDtoValidator>();

// Enregistre MediatR pour la gestion du pattern CQRS.
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(FlightDtoValidator).Assembly));

// Explorer API + routing.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Enregistre le service de health checks.
builder.Services.AddHealthChecks();

// Configure le versioning de l'API.
builder.Services
    .AddApiVersioning(options =>
    {
        // Version par défaut si aucune version n'est fournie.
        options.DefaultApiVersion = new ApiVersion(1, 0);

        // Si aucune version n'est précisée dans la requête, la version par défaut est utilisée.
        options.AssumeDefaultVersionWhenUnspecified = true;

        // Ajoute les versions supportées dans les headers de réponse.
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        // Format des groupes de documentation OpenAPI.
        options.GroupNameFormat = "'v'V";

        // Remplace le token de version dans les routes.
        options.SubstituteApiVersionInUrl = true;
    });

// Configure OpenAPI.
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "FlightNet REST API",
            Version = "v1",
            Description =
                "API REST professionnelle de gestion de vols, compagnies aériennes, aéroports, réservations, passagers, villes, pays et véhicules. " +
                "Cette documentation décrit les endpoints, les paramètres, les modèles d'entrée/sortie ainsi que les messages d'erreur standards.",
            Contact = new OpenApiContact
            {
                Name = "FlightNet API Team"
            }
        };

        document.Servers = new List<OpenApiServer>
        {
            new()
            {
                Url = "http://localhost:8080",
                Description = "Serveur local de développement"
            }
        };

        return Task.CompletedTask;
    });
});

// Enregistre les repositories et services métier.
builder.Services.AddRepoService();

// Récupère la configuration JWT depuis appsettings.json et/ou les variables d'environnement.
var jwtTokenConfig = builder.Configuration
    .GetSection("jwtTokenConfig")
    .Get<JwtTokenConfig>();

if (jwtTokenConfig is null)
{
    throw new InvalidOperationException(
        "La configuration 'jwtTokenConfig' est introuvable dans appsettings.json ou les variables d'environnement.");
}

// Enregistre l'authentification JWT.
builder.Services.AddJwtService(jwtTokenConfig);

// ============================================================
// Construction de l'application
// ============================================================

var app = builder.Build();

// ============================================================
// Migration automatique de la base de données au démarrage
// ============================================================

// Au démarrage, l'application tente d'appliquer automatiquement
// les migrations EF Core.
// Cela évite d'avoir à exécuter manuellement les migrations
// dans certains environnements Docker ou dev.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILoggerFactory>()
        .CreateLogger("StartupMigration");

    try
    {
        var dbContext = services.GetRequiredService<FlightContext>();
        dbContext.Database.Migrate();

        logger.LogInformation("Les migrations de la base de données ont été appliquées avec succès.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erreur lors de l'application des migrations de la base de données.");
        throw;
    }
}

// ============================================================
// Pipeline HTTP
// ============================================================

if (app.Environment.IsDevelopment())
{
    // Expose le document OpenAPI JSON.
    app.MapOpenApi("/openapi/{documentName}.json");

    // Expose l'interface Scalar.
    app.MapScalarApiReference("/scalar", options =>
    {
        options.WithTitle("FlightNet REST API Documentation")
               .WithOpenApiRoutePattern("/openapi/v1.json")
               .WithTheme(ScalarTheme.Solarized)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}
else
{
    // Active HSTS hors environnement de développement.
    app.UseHsts();
}

// Force la redirection HTTPS.
app.UseHttpsRedirection();

// Active le service de fichiers statiques si nécessaire.
app.UseStaticFiles();

// Applique la politique CORS.
app.UseCors("AllowAll");

// Active l'authentification puis l'autorisation.
app.UseAuthentication();
app.UseAuthorization();

// Mappe les contrôleurs API.
app.MapControllers();

// Endpoint de santé standard.
app.MapHealthChecks("/health");

// Endpoint racine utile pour vérifier rapidement que l'application répond.
app.MapGet("/", () => Results.Ok(new
{
    application = "FlightNet REST API",
    documentation = "/scalar",
    openApi = "/openapi/v1.json",
    version = "v1"
}))
.WithSummary("Point d'entrée de l'application")
.WithDescription("Retourne les informations de base de l'application et les liens de documentation.");

// Lance l'application.
await app.RunAsync();

/// <summary>
/// Classe partielle Program exposée pour les tests d'intégration via WebApplicationFactory.
/// </summary>
public partial class Program
{
}
