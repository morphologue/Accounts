using System.Threading;
using System.Threading.Tasks;
using Morphologue.Accounts.Application.Interfaces.Persistence;
using Morphologue.Accounts.CrossCutting.DependencyInjection;

namespace Morphologue.Accounts.Infrastructure.Persistence;

[ScopedService]
internal class UnitOfWork : IUnitOfWork
{
    private readonly AccountsDbContext _dbContext;
    private readonly IChangeTrackingFlags _flags;

    public UnitOfWork(AccountsDbContext dbContext, IChangeTrackingFlags flags)
    {
        _dbContext = dbContext;
        _flags = flags;
    }

    public void EnableChangeTracking() => _flags.IsChangeTrackingEnabled = true;

    public Task SaveChangesAsync(CancellationToken ct) => _dbContext.SaveChangesAsync(ct);
}