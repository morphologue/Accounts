using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Application.Transactions;

public class Transaction
{
    public Day Day { get; set; }
    public Amount Amount { get; set; }
    public Amount RunningTotal { get; set; }
    public string Description { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public string TemplateId { get; set; } = string.Empty;
    public bool IsRecurring { get; set; }
}
