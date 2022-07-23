namespace Morphologue.Accounts.Domain.Exceptions;

/// <summary>Thrown when a request cannot be understood</summary>
public class BadRequestException : UserException
{
    public BadRequestException(string message) : base(message)
    {
    }
}