using System;

namespace Morphologue.Accounts.Domain.Exceptions;

public abstract class UserException : Exception
{
    protected UserException(string message) : base(message)
    { }
}
