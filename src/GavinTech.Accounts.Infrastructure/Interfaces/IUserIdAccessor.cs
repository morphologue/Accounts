using System;

namespace GavinTech.Accounts.Infrastructure.Interfaces;

public interface IUserIdAccessor
{
    public Guid UserId { get; }
}