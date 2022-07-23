namespace Morphologue.Accounts.Domain.Exceptions;

/// <summary>Thrown when a given identifier does not match any entity instance</summary>
public class NotFoundException : UserException
{
    public NotFoundException(string message) : base(message)
    {
    }
}