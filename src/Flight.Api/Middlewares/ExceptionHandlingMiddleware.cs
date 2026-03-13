using System.Net;
using System.Text.Json;

namespace Flight.Api.Middlewares;

/// <summary>
/// Middleware global chargé d'intercepter les exceptions non gérées
/// et de retourner une réponse JSON cohérente au client.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initialise une nouvelle instance du middleware de gestion d'erreurs.
    /// </summary>
    /// <param name="next">Le middleware suivant dans le pipeline.</param>
    /// <param name="logger">Le logger injecté par le conteneur DI.</param>
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Exécute le middleware et intercepte les exceptions.
    /// </summary>
    /// <param name="context">Le contexte HTTP courant.</param>
    /// <returns>Une tâche asynchrone représentant l'exécution du middleware.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Une erreur non gérée est survenue.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var payload = new
            {
                success = false,
                message = "Une erreur interne est survenue.",
                detail = exception.Message,
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}