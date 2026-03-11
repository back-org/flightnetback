namespace Flight.Domain.Entities;

/// <summary>
/// Représente le genre (sexe) d'un passager.
/// </summary>
public enum Genre
{
    /// <summary>Masculin.</summary>
    Male,

    /// <summary>Féminin.</summary>
    Female,

    /// <summary>Non spécifié ou inconnu.</summary>
    Unknown
}

/// <summary>
/// Représente la classe de confort disponible dans un vol.
/// </summary>
public enum Confort
{
    /// <summary>Classe affaires.</summary>
    Business,

    /// <summary>Classe économique standard.</summary>
    Economy,

    /// <summary>Classe confort (entre économique et affaires).</summary>
    Comfort,

    /// <summary>Classe de luxe.</summary>
    Deluxe
}

/// <summary>
/// Représente le statut d'une réservation de vol.
/// </summary>
public enum Statut
{
    /// <summary>Réservation en attente de confirmation.</summary>
    Pending,

    /// <summary>Réservation confirmée.</summary>
    Confirmed,

    /// <summary>Réservation annulée.</summary>
    Cancelled
}

/// <summary>
/// Représente l'état d'activité d'une compagnie aérienne ou d'un aéroport.
/// </summary>
public enum State
{
    /// <summary>Entité active et opérationnelle.</summary>
    Active,

    /// <summary>Entité inactive ou suspendue.</summary>
    Inactive
}
