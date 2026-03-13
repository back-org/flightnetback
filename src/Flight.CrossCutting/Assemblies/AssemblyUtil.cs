using System.Collections.Generic;
using System.Reflection;

namespace Flight.CrossCutting.Assemblies;

/// <summary>
///     The assembly util.
/// </summary>
public static class AssemblyUtil
{
    /// <summary>
    ///     Gets the current assemblies.
    /// </summary>
    /// <returns>A list of Assemblies.</returns>
    public static IEnumerable<Assembly> GetCurrentAssemblies()
    {
        return
        [
            Assembly.Load("Flight.Api"),
            Assembly.Load("Flight.Application"),
            Assembly.Load("Flight.Domain"),
            Assembly.Load("Flight.Domain.Core"),
            Assembly.Load("Flight.Infrastructure"),
            Assembly.Load("Flight.CrossCutting")
        ];
    }
}