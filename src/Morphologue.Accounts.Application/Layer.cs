using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Morphologue.Accounts.Application.Templates;
using Morphologue.Accounts.CrossCutting.DependencyInjection;

namespace Morphologue.Accounts.Application;

public class Layer : ScanningLayerBase
{
    public override void RegisterDependencies(ServiceCollection services)
    {
        base.RegisterDependencies(services);
        services.AddMediatR(config => config.AsScoped(), typeof(Layer).Assembly);
        services.AddScoped(typeof(ITemplateWriter<>), typeof(TemplateWriter<>));
    }
}