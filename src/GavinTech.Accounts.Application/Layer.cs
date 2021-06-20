using GavinTech.Accounts.CrossCutting.DependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GavinTech.Accounts.Application
{
    public class Layer : ScanningLayerBase
    {
        public override void RegisterDependencies(ServiceCollection services)
        {
            base.RegisterDependencies(services);
            services.AddMediatR(config => config.AsScoped(), typeof(Layer).Assembly);
        }
    }
}
