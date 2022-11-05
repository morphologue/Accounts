using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Domain.Entities;

public class TransactionTemplate : IEntity
{
    public Day Day { get; set; }
    public Amount Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public Account Account { get; set; } = new();
}
