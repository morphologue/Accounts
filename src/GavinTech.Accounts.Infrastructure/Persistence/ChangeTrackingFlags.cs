using GavinTech.Accounts.CrossCutting.DependencyInjection;

namespace GavinTech.Accounts.Infrastructure.Persistence
{
    public interface IChangeTrackingFlags
    {
        bool IsChangeTrackingEnabled { get; set; }
    }

    [ScopedService]
    internal class ChangeTrackingFlags : IChangeTrackingFlags
    {
        public bool IsChangeTrackingEnabled { get; set; }
    }
}
