using Microsoft.Extensions.DependencyInjection;
using Fluxless.Blazor.Core;

namespace Fluxless.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Store<T> subclasses in DI automatically.
    /// </summary>
    public static IServiceCollection AddFluxlessStores(this IServiceCollection services)
    {
        var storeTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract &&
                        t.BaseType is { IsGenericType: true } &&
                        t.BaseType.GetGenericTypeDefinition() == typeof(Store<>));

        foreach (var type in storeTypes)
        {
            services.AddSingleton(type);
        }

        return services;
    }
}
