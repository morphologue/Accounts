using GavinTech.Accounts.Application.Interfaces.Persistence;
using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Infrastructure.Persistence
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly AccountsDbContext _dbContext;
        private readonly Layer.Options _layerOptions;
        private readonly Func<IUserIdAccessor> _userIdAccessorGetter;
        private readonly IChangeTrackingFlags _flags;

        public Repository(
            AccountsDbContext dbContext,
            Layer.Options layerOptions,
            Func<IUserIdAccessor> userIdAccessorGetter,
            IChangeTrackingFlags flags)
        {
            _dbContext = dbContext;
            _dbSet = (DbSet<TEntity>)(_dbContext
                .GetType()
                .GetProperties()
                .Where(p => typeof(DbSet<TEntity>).IsAssignableTo(p.PropertyType))
                .Single()
                .GetValue(dbContext) ?? throw new ApplicationException("DB context not populated"));
            _layerOptions = layerOptions;
            _userIdAccessorGetter = userIdAccessorGetter;
            _flags = flags;
        }

        public Task<List<TEntity>> GetAsync(CancellationToken ct, Expression<Func<TEntity, bool>>? predicate = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (!_flags.IsChangeTrackingEnabled)
            {
                query = query.AsNoTracking();
            }

            // Include one level of navigation properties.
            foreach (var property in _dbSet.EntityType.GetNavigations())
            {
                query = query.Include(property.Name);
            }

            if (_layerOptions.IsMultiUser)
            {
                var userId = _userIdAccessorGetter().UserId;
                query = query.Where(row => EF.Property<Guid>(row, Constants.UserIdColumnName) == userId);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.ToListAsync(ct);
        }

        public void Add(TEntity entity)
        {
            RequireChangeTracking();
            _dbSet.Add(entity);
            if (_layerOptions.IsMultiUser)
            {
                var userId = _userIdAccessorGetter().UserId;
                _dbContext.Entry(entity).Property(Constants.UserIdColumnName).CurrentValue = userId;
            }
        }

        public void Delete(TEntity entity)
        {
            RequireChangeTracking();
            _dbSet.Remove(entity);
        }

        private void RequireChangeTracking()
        {
            if (!_flags.IsChangeTrackingEnabled)
            {
                throw new InvalidOperationException("Mutation attempted while change tracking disabled");
            }
        }
    }
}
