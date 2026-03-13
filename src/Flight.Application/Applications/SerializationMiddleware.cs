using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Flight.Application.Applications;

/// <summary>
///     The json service.
/// </summary>
public static class SerializationMiddleware
{
    /// <summary>
    ///     Adds the json formatter.
    /// </summary>
    /// <param name="services">The services.</param>
    public static void AddJsonFormatter(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(opts =>
        {
            opts.JsonSerializerOptions.WriteIndented = true;
            opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            opts.JsonSerializerOptions.NumberHandling = JsonNumberHandling.WriteAsString;
            opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    }
}