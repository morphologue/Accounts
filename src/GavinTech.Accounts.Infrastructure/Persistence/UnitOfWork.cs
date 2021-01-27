using System;
using System.Threading.Tasks;
using GavinTech.Accounts.Application.DependencyInjection;
using GavinTech.Accounts.Application.Interfaces.Infrastructure.Persistence;

namespace GavinTech.Accounts.Infrastructure.Persistence
{
    [ScopedService]
    public class UnitOfWork : IUnitOfWork
    {
        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
