using System;
using System.Threading;
using System.Threading.Tasks;
using GavinTech.Accounts.Application.DependencyInjection;
using GavinTech.Accounts.Application.Interfaces.Infrastructure.Persistence;

namespace GavinTech.Accounts.Infrastructure.Persistence
{
    [ScopedService]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AccountsDbContext _dbContext;

        public UnitOfWork(AccountsDbContext dbContext) => _dbContext = dbContext;

        public Task SaveChangesAsync(CancellationToken ct) => _dbContext.SaveChangesAsync(ct);
    }
}
