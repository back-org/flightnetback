namespace Flight.Api.Models;

/// <summary>
/// Représente un format de réponse standard pour l'API.
/// </summary>
/// <typeparam name="T">Le type de données retourné.</typeparam>
public sealed class ApiResponse<T>
{
    /// <summary>
    /// Indique si l'opération a réussi.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Message métier ou technique associé à la réponse.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Données retournées par l'opération.
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// Identifiant de corrélation ou de trace de la requête.
    /// </summary>
    public string? TraceId { get; init; }

    /// <summary>
    /// Construit une réponse de succès.
    /// </summary>
    /// <param name="data">Les données à retourner.</param>
    /// <param name="message">Le message associé.</param>
    /// <param name="traceId">L'identifiant de trace.</param>
    /// <returns>Une instance de <see cref="ApiResponse{T}"/>.</returns>
    public static ApiResponse<T> Ok(T? data, string message = "Opération réussie.", string? traceId = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            TraceId = traceId
        };
    }

    /// <summary>
    /// Construit une réponse d'échec.
    /// </summary>
    /// <param name="message">Le message d'erreur.</param>
    /// <param name="traceId">L'identifiant de trace.</param>
    /// <returns>Une instance de <see cref="ApiResponse{T}"/>.</returns>
    public static ApiResponse<T> Fail(string message, string? traceId = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            TraceId = traceId
        };
    }
}