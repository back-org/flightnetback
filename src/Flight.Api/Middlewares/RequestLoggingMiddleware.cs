using System.Diagnostics;

namespace Flight.Api.Middlewares;

/// <summary>
/// Middleware chargé de journaliser chaque requête HTTP entrante
/// avec sa durée d'exécution et son code de réponse.
/// </summary>
public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    /// <summary>
    /// Initialise une nouvelle instance du middleware de journalisation.
    /// </summary>
    /// <param name="next">Le middleware suivant.</param>
    /// <param name="logger">Le logger injecté.</param>
    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Journalise la requête HTTP courante.
    /// </summary>
    /// <param name="context">Le contexte HTTP.</param>
    /// <returns>Une tâche asynchrone.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        _logger.LogInformation(
            "HTTP {Method} {Path} => {StatusCode} en {ElapsedMs} ms | TraceId: {TraceId}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds,
            context.TraceIdentifier);
    }
}