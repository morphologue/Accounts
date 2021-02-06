using System;
using System.Threading.Tasks;
using GavinTech.Accounts.Application.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace GavinTech.Accounts.Application
{
    public class Layer : ILayer
    {
        public void RegisterDependencies(ServiceCollection services, string[] args)
        {
        }

        public Task InitialiseAsync(IServiceProvider scopedProvider) => Task.CompletedTask;
    }
}
