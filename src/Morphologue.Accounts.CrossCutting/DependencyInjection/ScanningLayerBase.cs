using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Morphologue.Accounts.CrossCutting.DependencyInjection;

public abstract class ScanningLayerBase : ILayer
{
    public virtual void RegisterDependencies(ServiceCollection services)
    {
        // Must get this outside the delegate!
        var callingAssembly = Assembly.GetCallingAssembly();
        services.Scan(scan => scan.FromAssemblies(callingAssembly)
            .AddClasses(classes => classes.WithAttribute<SingletonServiceAttribute>(), publicOnly: false)
            .AsImplementedInterfaces().WithSingletonLifetime()
            .AddClasses(classes => classes.WithAttribute<ScopedServiceAttribute>(), publicOnly: false)
            .AsImplementedInterfaces().WithScopedLifetime());
    }

    public virtual Task InitialiseAsync(IServiceProvider scopedProvider, CancellationToken ct) => Task.CompletedTask;
}
