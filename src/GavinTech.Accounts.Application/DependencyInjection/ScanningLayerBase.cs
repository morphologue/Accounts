using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace GavinTech.Accounts.Application.DependencyInjection
{
    public abstract class ScanningLayerBase : ILayer
    {
        public virtual void RegisterDependencies(ServiceCollection services, string[] args)
        {
            // Must get this outside the delegate!
            var callingAssembly = Assembly.GetCallingAssembly();
            services.Scan(scan => scan.FromAssemblies(callingAssembly)
                .AddClasses(classes => classes.WithAttribute<SingletonServiceAttribute>())
                    .AsImplementedInterfaces().WithSingletonLifetime()
                .AddClasses(classes => classes.WithAttribute<ScopedServiceAttribute>())
                    .AsImplementedInterfaces().WithScopedLifetime());
        }
    }
}
