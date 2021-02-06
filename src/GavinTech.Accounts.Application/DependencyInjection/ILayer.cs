using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GavinTech.Accounts.Application.DependencyInjection
{
    public interface ILayer
    {
        void RegisterDependencies(ServiceCollection services, string[] args);
        Task InitialiseAsync(IServiceProvider scopedProvider);
    }
}
