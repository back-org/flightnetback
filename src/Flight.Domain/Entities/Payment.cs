using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente un paiement effectué pour une réservation.
/// </summary>
[Table("Payments")]
public class Payment
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Réservation concernée par le paiement.
    /// </summary>
    [Required]
    public int BookingId { get; set; }

    /// <summary>
    /// Montant payé.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Devise du paiement.
    /// </summary>
    [MaxLength(10)]
    public string Currency { get; set; } = "MGA";

    /// <summary>
    /// Méthode de paiement.
    /// Exemple : Cash, Card, MobileMoney, BankTransfer.
    /// </summary>
    [MaxLength(30)]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Statut du paiement.
    /// Exemple : Pending, Paid, Failed, Refunded.
    /// </summary>
    [MaxLength(30)]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Référence technique ou bancaire de la transaction.
    /// </summary>
    [MaxLength(100)]
    public string TransactionReference { get; set; } = string.Empty;

    /// <summary>
    /// Date de paiement.
    /// </summary>
    public DateTime? PaidAt { get; set; }
}