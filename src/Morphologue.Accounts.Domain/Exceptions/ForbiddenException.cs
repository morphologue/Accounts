namespace Morphologue.Accounts.Domain.Exceptions;

/// <summary>Thrown when a request is understood but not allowed</summary>
public class ForbiddenException : UserException
{
    public ForbiddenException(string message) : base(message)
    {
    }
}