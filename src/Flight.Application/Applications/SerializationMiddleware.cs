/*
 * Rôle métier du fichier: Orchestrer le pipeline d’exécution transverse (middleware, bootstrap, configuration).
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Applications' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Flight.Application.Applications;

/// <summary>
///     The json service.
/// </summary>
public static class SerializationMiddleware
{
    /// <summary>
    ///     Adds the json formatter.
    /// </summary>
    /// <param name="services">The services.</param>
    public static void AddJsonFormatter(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(opts =>
        {
            opts.JsonSerializerOptions.WriteIndented = true;
            opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            opts.JsonSerializerOptions.NumberHandling = JsonNumberHandling.WriteAsString;
            opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    }
}