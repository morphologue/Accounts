using GavinTech.Accounts.Application.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace GavinTech.Accounts.Presentation.ConsoleApp
{
    public class Layer : ScanningLayerBase
    {
        public override void RegisterDependencies(ServiceCollection services, string[] args)
        {
            // This apparently redundant override ensures that we are the
            // "calling" assembly.
            base.RegisterDependencies(services, args);
        }
    }
}
