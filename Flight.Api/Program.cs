using Flight.Api.Middlewares;
using Flight.Application.Applications;
using Flight.Application.Concrete;
using Flight.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Scalar.AspNetCore;
using Serilog;

namespace Flight.Api;

/// <summary>
/// Point d'entrée principal de l'API FlightNet.
/// Configure les services, la documentation OpenAPI, l'authentification,
/// la journalisation, le cache, la base de données et le pipeline HTTP.
/// </summary>
public static class Program
{
    /// <summary>
    /// Démarre l'application ASP.NET Core.
    /// </summary>
    /// <param name="args">Arguments de ligne de commande.</param>
    /// <returns>Une tâche représentant le cycle de vie de l'application.</returns>
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureLogging(builder);
        ConfigureServices(builder);

        var app = builder.Build();

        ConfigurePipeline(app);

        await app.RunAsync();
    }

    /// <summary>
    /// Configure Serilog comme fournisseur principal de logs.
    /// </summary>
    /// <param name="builder">Le builder de l'application.</param>
    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    /// <summary>
    /// Configure tous les services applicatifs.
    /// </summary>
    /// <param name="builder">Le builder de l'application.</param>
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDataContext(builder.Configuration);
        builder.Services.ConfigureCORS();
        builder.Services.AddRepoService();
        builder.Services.AddResponseCache();

        builder.Services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = false;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        var configManager = new ConfigManager();
        var jwtTokenConfig = configManager.AppSetting
            .GetSection("jwtTokenConfig")
            .Get<JwtTokenConfig>()
            ?? throw new InvalidOperationException(
                "La section 'jwtTokenConfig' est manquante dans la configuration.");

        builder.Services.AddJwtService(jwtTokenConfig);
    }

    /// <summary>
    /// Configure le pipeline HTTP de l'application.
    /// </summary>
    /// <param name="app">L'application construite.</param>
    private static void ConfigurePipeline(WebApplication app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
                options.WithTitle("FlightNet API")
                       .WithTheme(ScalarTheme.Solarized)
                       .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient));
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors();
        app.UseResponseCaching();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/health", () => Results.Ok(new
        {
            status = "Healthy",
            service = "FlightNet API",
            utc = DateTime.UtcNow
        }))
        .WithName("HealthCheck")
        .WithSummary("Retourne l'état de santé de l'API.");

        app.MapGet("/", () => Results.Ok(new
        {
            name = "FlightNet API",
            version = "v1",
            documentation = "/scalar/v1"
        }));
    }
}