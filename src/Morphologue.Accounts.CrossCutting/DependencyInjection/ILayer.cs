using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Morphologue.Accounts.CrossCutting.DependencyInjection;

public interface ILayer
{
    void RegisterDependencies(ServiceCollection services);
    Task InitialiseAsync(IServiceProvider scopedProvider, CancellationToken ct);
}
