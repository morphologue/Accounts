using System;

namespace GavinTech.Accounts.Domain.Exceptions
{
    public abstract class UserException : Exception
    {
        protected UserException(string message) : base(message)
        {
        }
    }
}
