using GavinTech.Accounts.CrossCutting.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Presentation.ConsoleApp
{
    internal class Program
    {
        private static async Task Main()
        {
            var provider = await new ILayer[]
            {
                new Application.Layer(),
                new Infrastructure.Layer(new()),
                new Presentation.ConsoleApp.Layer()
            }.BootstrapAsync();

            await provider.GetRequiredService<IRepl>().ExecuteAsync();
        }
    }
}
