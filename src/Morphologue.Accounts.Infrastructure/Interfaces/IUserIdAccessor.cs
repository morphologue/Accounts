using System;

namespace Morphologue.Accounts.Infrastructure.Interfaces;

public interface IUserIdAccessor
{
    public Guid UserId { get; }
}
