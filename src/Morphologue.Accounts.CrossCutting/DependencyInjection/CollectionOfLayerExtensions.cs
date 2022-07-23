using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Morphologue.Accounts.CrossCutting.DependencyInjection;

public static class CollectionOfLayerExtensions
{
    public static async Task<IServiceProvider> BootstrapAsync(this IReadOnlyCollection<ILayer> layers, CancellationToken ct)
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
            await layer.InitialiseAsync(scope.ServiceProvider, ct);
        }

        return built;
    }
}
