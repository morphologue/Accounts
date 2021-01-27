using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GavinTech.Accounts.Presentation.ConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var provider = WireUpLayers(args);
            await provider.GetRequiredService<IRepl>().ExecuteAsync();
        }

        private static IServiceProvider WireUpLayers(string[] args)
        {
            Application.DependencyInjection.ILayer[] layers =
            {
                // Later service registrations override earlier ones, so the
                // below ordering means that higher layers take precedence.
                new Application.DependencyInjection.Layer(),
                new Infrastructure.DependencyInjection.Layer(),
                new Presentation.ConsoleApp.DependencyInjection.Layer()
            };

            var services = new ServiceCollection();
            foreach (var layer in layers)
            {
                layer.RegisterDependencies(services, args);
            }
            return services.BuildServiceProvider();
        }
    }
}
