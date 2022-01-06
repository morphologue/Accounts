using GavinTech.Accounts.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace GavinTech.Accounts.Infrastructure.Persistence.EntityIdentification;

public interface IEntityIdentifier<TEntity>
    where TEntity : class, IEntity
{
    string Identify(TEntity entity);

    Expression<Func<TEntity, bool>> MakePredicate(string id);
}