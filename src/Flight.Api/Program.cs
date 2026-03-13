using Asp.Versioning;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using Flight.Application.Applications;
using Flight.Application.Validators;
using Flight.Infrastructure.Auth;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Scalar.AspNetCore;

// Charge les variables définies dans le fichier .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Ajoute aussi les variables d'environnement système à la configuration
builder.Configuration.AddEnvironmentVariables();

// -----------------------------
// Enregistrement des services
// -----------------------------

// Base de données
builder.Services.AddDataContext(builder.Configuration);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Contrôleurs + sérialisation JSON
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<FlightDtoValidator>();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(FlightDtoValidator).Assembly));

// Routing / API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// API Versioning
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

// OpenAPI
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
            new OpenApiServer
            {
                Url = "http://localhost:8080",
                Description = "Serveur local de développement"
            }
        };

        return Task.CompletedTask;
    });
});

// Repositories / services métier
builder.Services.AddRepoService();

// JWT
var jwtTokenConfig = builder.Configuration
    .GetSection("jwtTokenConfig")
    .Get<JwtTokenConfig>();

if (jwtTokenConfig is null)
{
    throw new InvalidOperationException(
        "La configuration 'jwtTokenConfig' est introuvable dans appsettings.json ou les variables d'environnement.");
}

builder.Services.AddJwtService(jwtTokenConfig);

var app = builder.Build();

// -----------------------------
// Pipeline HTTP
// -----------------------------

if (app.Environment.IsDevelopment())
{
    // Expose le document OpenAPI JSON
    app.MapOpenApi("/openapi/{documentName}.json");

    // Expose l'interface Scalar
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
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
    application = "FlightNet REST API",
    documentation = "/scalar",
    openApi = "/openapi/v1.json",
    version = "v1"
}))
.WithSummary("Point d'entrée de l'application")
.WithDescription("Retourne les informations de base de l'application et les liens de documentation.");

app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    application = "FlightNet REST API"
}))
.WithSummary("Point de contrôle de santé")
.WithDescription("Retourne l'état de santé de l'application.");

await app.RunAsync();

public partial class Program
{
}
