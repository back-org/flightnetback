/*
 * Rôle métier du fichier: Transporter les données métier entre couches sans exposer les entités internes.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/DTOs' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant un paiement effectué pour une réservation.
/// </summary>
public class PaymentDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide du DTO paiement.
    /// </summary>
    public PaymentDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance du DTO paiement.
    /// </summary>
    public PaymentDto(
        int id,
        int bookingId,
        decimal amount,
        string currency,
        string paymentMethod,
        string status,
        string transactionReference,
        DateTime? paidAt)
    {
        Id = id;
        BookingId = bookingId;
        Amount = amount;
        Currency = currency;
        PaymentMethod = paymentMethod;
        Status = status;
        TransactionReference = transactionReference;
        PaidAt = paidAt;
    }

    /// <summary>
    /// Identifiant unique du paiement.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifiant de la réservation liée au paiement.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant de la réservation est requis.")]
    public int BookingId { get; set; }

    /// <summary>
    /// Montant du paiement.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Le montant doit être positif.")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Devise utilisée.
    /// Exemple : MGA, EUR, USD.
    /// </summary>
    [MaxLength(10, ErrorMessage = "La devise ne peut pas dépasser 10 caractères.")]
    public string Currency { get; set; } = "MGA";

    /// <summary>
    /// Méthode de paiement utilisée.
    /// Exemple : Cash, Card, MobileMoney, BankTransfer.
    /// </summary>
    [MaxLength(30, ErrorMessage = "La méthode de paiement ne peut pas dépasser 30 caractères.")]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Statut du paiement.
    /// Exemple : Pending, Paid, Failed, Refunded.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le statut ne peut pas dépasser 30 caractères.")]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Référence technique de la transaction.
    /// </summary>
    [MaxLength(100, ErrorMessage = "La référence de transaction ne peut pas dépasser 100 caractères.")]
    public string TransactionReference { get; set; } = string.Empty;

    /// <summary>
    /// Date à laquelle le paiement a été effectué.
    /// </summary>
    public DateTime? PaidAt { get; set; }
}