using System.Collections.Generic;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Domain.Entities;

public class RecurringTransactionTemplate : TransactionTemplate
{
    public RecurrenceBasis Basis { get; set; }
    public uint Multiplicand { get; set; }
    public Day? UntilExcl { get; set; }
    public HashSet<Day> Tombstones { get; set; } = new HashSet<Day>();
}

public enum RecurrenceBasis
{
    Daily,
    Monthly
}
