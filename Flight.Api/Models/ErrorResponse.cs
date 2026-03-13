namespace Flight.Api.Models;

/// <summary>
/// Représente un format standard d'erreur retourné par l'API.
/// </summary>
public sealed class ErrorResponse
{
    /// <summary>
    /// Code HTTP associé à l'erreur.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Message principal décrivant l'erreur.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Détail complémentaire utile au diagnostic.
    /// </summary>
    public string? Detail { get; set; }

    /// <summary>
    /// Identifiant de traçabilité de la requête.
    /// </summary>
    public string? TraceId { get; set; }
}