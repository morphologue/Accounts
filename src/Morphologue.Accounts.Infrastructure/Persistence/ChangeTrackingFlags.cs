using Morphologue.Accounts.CrossCutting.DependencyInjection;

namespace Morphologue.Accounts.Infrastructure.Persistence;

public interface IChangeTrackingFlags
{
    bool IsChangeTrackingEnabled { get; set; }
}

[ScopedService]
internal class ChangeTrackingFlags : IChangeTrackingFlags
{
    public bool IsChangeTrackingEnabled { get; set; }
}