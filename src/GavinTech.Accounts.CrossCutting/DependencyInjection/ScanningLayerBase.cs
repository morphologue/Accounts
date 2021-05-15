using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace GavinTech.Accounts.CrossCutting.DependencyInjection
{
    public abstract class ScanningLayerBase : ILayer
    {
        public virtual void RegisterDependencies(ServiceCollection services)
        {
            // Must get this outside the delegate!
            var callingAssembly = Assembly.GetCallingAssembly();
            services.Scan(scan => scan.FromAssemblies(callingAssembly)
                .AddClasses(classes => classes.WithAttribute<SingletonServiceAttribute>())
                    .AsImplementedInterfaces().WithSingletonLifetime()
                .AddClasses(classes => classes.WithAttribute<ScopedServiceAttribute>())
                    .AsImplementedInterfaces().WithScopedLifetime());
        }

        public virtual Task InitialiseAsync(IServiceProvider scopedProvider) => Task.CompletedTask;
    }
}
