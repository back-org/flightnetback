namespace Flight.Api.Models;

/// <summary>
/// Représente un message d'erreur standard retourné par l'API.
/// </summary>
public sealed class ErrorResponse
{
    /// <summary>
    /// Code HTTP associé à l'erreur.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Message lisible décrivant l'erreur.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Détail technique complémentaire, si disponible.
    /// </summary>
    public string? Detail { get; set; }

    /// <summary>
    /// Identifiant de corrélation ou de trace de la requête.
    /// </summary>
    public string? TraceId { get; set; }
}