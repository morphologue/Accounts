using GavinTech.Accounts.CrossCutting.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace GavinTech.Accounts.Presentation.ConsoleApp;

public class Layer : ScanningLayerBase
{
    public override void RegisterDependencies(ServiceCollection services)
    {
        // This apparently redundant override ensures that we are the
        // "calling" assembly.
        base.RegisterDependencies(services);
    }
}
