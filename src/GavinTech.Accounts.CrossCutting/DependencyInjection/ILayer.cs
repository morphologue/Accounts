using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace GavinTech.Accounts.CrossCutting.DependencyInjection;

public interface ILayer
{
    void RegisterDependencies(ServiceCollection services);
    Task InitialiseAsync(IServiceProvider scopedProvider);
}