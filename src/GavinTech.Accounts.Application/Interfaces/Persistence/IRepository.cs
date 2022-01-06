using GavinTech.Accounts.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Interfaces.Persistence;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    Task<List<TEntity>> GetAsync(CancellationToken ct);
    Task<TEntity?> GetAsync(string identifier, CancellationToken ct);
    string Identify(TEntity entity);
    void Add(TEntity entity);
    void Delete(TEntity entity);
    void Delete(IEnumerable<TEntity> entities);
}