/*
 * Rôle métier du fichier: Composant applicatif.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain.Core/Abstracts' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿using System.ComponentModel.DataAnnotations.Schema;
using Flunt.Notifications;

namespace Flight.Domain.Core.Abstracts;

[NotMapped]
public class BaseNotification : Notification
{
    public int Id { get; set; }
}