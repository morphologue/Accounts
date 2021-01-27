using Microsoft.Extensions.DependencyInjection;

namespace GavinTech.Accounts.Application.DependencyInjection
{
    public interface ILayer
    {
        void RegisterDependencies(ServiceCollection services, string[] args);
    }
}
