using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Flight.Application.Applications;

/// <summary>
/// Interface du service utilisateur.
/// Fournit les méthodes de validation des identifiants et d'attribution des rôles.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Vérifie si un utilisateur avec le nom donné existe dans le système.
    /// </summary>
    /// <param name="userName">Le nom d'utilisateur à vérifier.</param>
    /// <returns><c>true</c> si l'utilisateur existe, <c>false</c> sinon.</returns>
    bool IsAnExistingUser(string userName);

    /// <summary>
    /// Vérifie si les identifiants fournis (nom d'utilisateur + mot de passe) sont valides.
    /// </summary>
    /// <param name="userName">Le nom d'utilisateur.</param>
    /// <param name="password">Le mot de passe à valider.</param>
    /// <returns><c>true</c> si les identifiants sont corrects, <c>false</c> sinon.</returns>
    bool IsValidUserCredentials(string userName, string password);

    /// <summary>
    /// Retourne le rôle assigné à l'utilisateur spécifié.
    /// </summary>
    /// <param name="userName">Le nom d'utilisateur.</param>
    /// <returns>Le nom du rôle (ex : "Admin", "BasicUser"), ou <see cref="string.Empty"/> si l'utilisateur n'existe pas.</returns>
    string GetUserRole(string userName);
}

/// <summary>
/// Implémentation du service utilisateur utilisant un dictionnaire en mémoire.
/// </summary>
/// <remarks>
/// <b>Note :</b> Cette implémentation est uniquement à des fins de démonstration.
/// En production, remplacez ce dictionnaire par une source de données persistante
/// (base de données, LDAP, Azure AD, etc.) avec hachage des mots de passe (BCrypt, Argon2).
/// </remarks>
public class UserService(ILogger<UserService> logger) : IUserService
{
    /// <summary>
    /// Dictionnaire en mémoire simulant un magasin d'utilisateurs.
    /// La clé est le nom d'utilisateur, la valeur est le mot de passe en clair.
    /// <b>Ne pas utiliser en production.</b>
    /// </summary>
    private readonly Dictionary<string, string> _users = new()
    {
        { "test1", "password1" },
        { "test2", "password2" },
        { "admin", "securePassword" }
    };

    /// <inheritdoc/>
    public bool IsValidUserCredentials(string userName, string password)
    {
        logger.LogInformation("Validation des identifiants pour l'utilisateur [{UserName}].", userName);

        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            return false;

        return _users.TryGetValue(userName, out var storedPassword) && storedPassword == password;
    }

    /// <inheritdoc/>
    public bool IsAnExistingUser(string userName) => _users.ContainsKey(userName);

    /// <inheritdoc/>
    public string GetUserRole(string userName)
    {
        if (!IsAnExistingUser(userName))
            return string.Empty;

        return userName == "admin" ? UserRoles.Admin : UserRoles.BasicUser;
    }
}

/// <summary>
/// Constantes représentant les rôles disponibles dans l'application.
/// </summary>
public static class UserRoles
{
    /// <summary>Rôle administrateur — accès complet à toutes les ressources.</summary>
    public const string Admin = nameof(Admin);

    /// <summary>Rôle utilisateur de base — accès en lecture et opérations limitées.</summary>
    public const string BasicUser = nameof(BasicUser);
}
