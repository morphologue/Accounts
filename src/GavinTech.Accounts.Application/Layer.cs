using GavinTech.Accounts.CrossCutting.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application
{
    public class Layer : ILayer
    {
        public void RegisterDependencies(ServiceCollection services) { }

        public Task InitialiseAsync(IServiceProvider scopedProvider) => Task.CompletedTask;
    }
}
