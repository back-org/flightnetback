using Flight.Application.Applications;
using Flight.Application.Concrete;
using Flight.Infrastructure.Auth;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataContext();
builder.Services.ConfigureCORS();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

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

builder.Services.AddRepoService();

var config = new ConfigManager();
var jwtTokenConfig = config.AppSetting
    .GetSection("jwtTokenConfig")
    .Get<JwtTokenConfig>();

if (jwtTokenConfig is null)
{
    throw new InvalidOperationException("La configuration jwtTokenConfig est introuvable.");
}

builder.Services.AddJwtService(jwtTokenConfig);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/{documentName}.json");

    app.MapScalarApiReference("/scalar/{documentName}", options =>
    {
        options.WithTitle("FlightNet REST API Documentation")
            .WithTheme(ScalarTheme.Solarized)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
    application = "FlightNet REST API",
    documentation = "/scalar/v1",
    openApi = "/openapi/v1.json"
}))
.WithSummary("Point d'entrée de l'application")
.WithDescription("Retourne les informations de base de l'application et les liens de documentation.");

await app.RunAsync();