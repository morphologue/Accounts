using System;
using System.Linq.Expressions;
using Morphologue.Accounts.Domain.Entities;

namespace Morphologue.Accounts.Infrastructure.Persistence.EntityIdentification;

public interface IEntityIdentifier<TEntity>
    where TEntity : class, IEntity
{
    string Identify(TEntity entity);

    Expression<Func<TEntity, bool>> MakePredicate(string id);
}