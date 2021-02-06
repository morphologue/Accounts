using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GavinTech.Accounts.Presentation.ConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var provider = await InitialiseLayersAsync(args);
            await provider.GetRequiredService<IRepl>().ExecuteAsync();
        }

        private static async Task<IServiceProvider> InitialiseLayersAsync(string[] args)
        {
            Application.DependencyInjection.ILayer[] layers =
            {
                // Later service registrations override earlier ones, so the
                // below ordering means that higher layers take precedence.
                new Application.Layer(),
                new Infrastructure.Layer(),
                new Presentation.ConsoleApp.Layer()
            };

            var services = new ServiceCollection();
            foreach (var layer in layers)
            {
                layer.RegisterDependencies(services, args);
            }
            var built = services.BuildServiceProvider();

            using var scope = built.GetRequiredService<IServiceScopeFactory>().CreateScope();
            foreach (var layer in layers)
            {
                await layer.InitialiseAsync(scope.ServiceProvider);
            }

            return built;
        }
    }
}
