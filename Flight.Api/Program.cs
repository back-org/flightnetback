using Microsoft.AspNetCore.Builder;
using Flight.Application.Applications;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Scalar.AspNetCore;
using Flight.Application.Concrete;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Flight.Infrastructure.Auth;

/*
 * ============================================================
 * FlightNet Backend — Point d'entrée de l'application ASP.NET
 * ============================================================
 * Configure le pipeline HTTP, les services DI et démarre le serveur.
 * L'ordre de configuration middleware est important et doit être respecté.
 */

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────
// 1. Base de données (MySQL via Entity Framework)
// ──────────────────────────────────────────────
builder.Services.AddDataContext(builder.Configuration);

// ──────────────────────────────────────────────
// 2. CORS (politique par défaut : AllowAll)
// ──────────────────────────────────────────────
builder.Services.ConfigureCORS();

// ──────────────────────────────────────────────
// 3. Contrôleurs MVC + sérialisation JSON
// ──────────────────────────────────────────────
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNamingPolicy = null);

// ──────────────────────────────────────────────
// 4. OpenAPI / Scalar (documentation interactive)
// ──────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// ──────────────────────────────────────────────
// 5. Dépôts, services et cache
// ──────────────────────────────────────────────
builder.Services.AddRepoService();
builder.Services.AddResponseCache();

// ──────────────────────────────────────────────
// 6. Authentification JWT
// ──────────────────────────────────────────────
var configManager = new ConfigManager();
var jwtTokenConfig = configManager.AppSetting
    .GetSection("jwtTokenConfig")
    .Get<JwtTokenConfig>()
    ?? throw new InvalidOperationException(
        "La section 'jwtTokenConfig' est manquante dans appsettings.json.");

builder.Services.AddJwtService(jwtTokenConfig);

// ──────────────────────────────────────────────
// Construction de l'application
// ──────────────────────────────────────────────
var app = builder.Build();

// ──────────────────────────────────────────────
// Pipeline HTTP — Développement
// ──────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHsts();
    app.MapScalarApiReference(options =>
        options.WithTheme(ScalarTheme.Solarized)
               .WithTitle("FlightNet — API REST ASP.NET Core")
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient));
}

// ──────────────────────────────────────────────
// Pipeline HTTP — Middlewares
// ──────────────────────────────────────────────
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();
app.UseResponseCaching();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
