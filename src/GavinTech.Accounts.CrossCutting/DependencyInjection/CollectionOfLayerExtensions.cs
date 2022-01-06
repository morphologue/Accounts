using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GavinTech.Accounts.CrossCutting.DependencyInjection;

public static class CollectionOfLayerExtensions
{
    public static async Task<IServiceProvider> BootstrapAsync(this IReadOnlyCollection<ILayer> layers)
    {
        var services = new ServiceCollection();
        foreach (var layer in layers)
        {
            layer.RegisterDependencies(services);
        }
        var built = services.BuildServiceProvider(validateScopes: true);

        using var scope = built.GetRequiredService<IServiceScopeFactory>().CreateScope();
        foreach (var layer in layers)
        {
            await layer.InitialiseAsync(scope.ServiceProvider);
        }

        return built;
    }
}