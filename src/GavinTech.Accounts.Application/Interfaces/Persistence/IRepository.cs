using GavinTech.Accounts.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Interfaces.Persistence
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<List<TEntity>> GetAllAsync(CancellationToken ct);
        void Add(TEntity entity);
        void Delete(TEntity entity);
    }
}
