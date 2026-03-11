using System.Threading.RateLimiting;
using Flight.Api.Authorization;
using Flight.Api.Middlewares;
using Flight.Application.Applications;
using Flight.Application.Behaviors;
using Flight.Application.Concrete;
using Flight.Application.Validators;
using Flight.Infrastructure.Auth;
using Flight.Infrastructure.AuditTrail;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json.Serialization;
using Scalar.AspNetCore;
using Serilog;
using Flight.Infrastructure.Interfaces;
using Asp.Versioning;

namespace Flight.Api;

/// <summary>
/// Point d'entrée principal de l'API FlightNet.
/// Configure les services, la documentation OpenAPI, l'authentification,
/// la journalisation, le cache, la base de données et le pipeline HTTP.
/// </summary>
public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureLogging(builder);
        ConfigureServices(builder);
        var app = builder.Build();
        ConfigurePipeline(app);
        await app.RunAsync();
    }

    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        builder.Host.UseSerilog();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDataContext(builder.Configuration);
        builder.Services.ConfigureCORS();
        builder.Services.AddRepoService();
        builder.Services.AddResponseCache();
        builder.Services.AddHttpContextAccessor();

        // Audit Trail
        builder.Services.AddScoped<IAuditTrailService, AuditTrailService>();

        // MediatR + CQRS + Pipeline Behaviors
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                typeof(Flight.Application.CQRS.Queries.Flights.GetAllFlightsQuery).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // FluentValidation
        builder.Services.AddValidatorsFromAssemblyContaining<FlightDtoValidator>();

        // Controllers
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

        // API Versioning
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-API-Version"),
                new QueryStringApiVersionReader("api-version"));
        });

        // deprecated.
        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        // Rate Limiting
        builder.Services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ctx.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 5
                    }));

            options.AddFixedWindowLimiter("auth", o =>
            {
                o.PermitLimit = 5;
                o.Window = TimeSpan.FromMinutes(5);
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 0;
            });

            options.AddSlidingWindowLimiter("write", o =>
            {
                o.PermitLimit = 30;
                o.Window = TimeSpan.FromMinutes(1);
                o.SegmentsPerWindow = 6;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 2;
            });

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.Headers["Retry-After"] = "60";
                await context.HttpContext.Response.WriteAsJsonAsync(new
                {
                    error = "Trop de requêtes. Veuillez réessayer plus tard.",
                    retryAfter = 60
                }, cancellationToken);
            };
        });

        // Fine-grained Authorization Policies
        builder.Services.AddSingleton<IAuthorizationHandler, ActiveAdminHandler>();
        builder.Services.AddSingleton<IAuthorizationHandler, ResourceOwnerOrAdminHandler>();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.ActiveAdmin,
                policy => policy.Requirements.Add(new ActiveAdminRequirement()));
            options.AddPolicy(AuthorizationPolicies.OwnerOrAdmin,
                policy => policy.Requirements.Add(new ResourceOwnerOrAdminRequirement()));
            options.AddPolicy(AuthorizationPolicies.WriteAccess,
                policy => policy.RequireRole("Admin", "BasicUser"));
            options.AddPolicy(AuthorizationPolicies.AuthenticatedRead,
                policy => policy.RequireAuthenticatedUser());
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        // JWT
        var configManager = new ConfigManager();
        var jwtTokenConfig = configManager.AppSetting
            .GetSection("jwtTokenConfig")
            .Get<JwtTokenConfig>()
            ?? throw new InvalidOperationException(
                "La section 'jwtTokenConfig' est manquante dans la configuration.");
        builder.Services.AddJwtService(jwtTokenConfig);
    }

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
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/health", () => Results.Ok(new
        {
            status = "Healthy",
            service = "FlightNet API",
            version = "v1",
            utc = DateTime.UtcNow
        })).WithName("HealthCheck").WithSummary("Retourne l'état de santé de l'API.");

        app.MapGet("/", () => Results.Ok(new
        {
            name = "FlightNet API",
            version = "v1",
            documentation = "/scalar/v1"
        }));
    }
}
