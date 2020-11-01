﻿using GavinTech.Accounts.Domain.Primitives;
using System.Collections.Generic;

namespace GavinTech.Accounts.Domain.Entities
{
    public enum RecurrenceBasis
    {
        Daily,
        Monthly
    }

    public class RecurringTransactionTemplate : TransactionTemplate
    {
        public RecurrenceBasis Basis { get; set; }
        public int Multiplicand { get; set; }
        public Day? UntilExcl { get; set; }
        public ICollection<Day> Tombstones { get; set; } = new HashSet<Day>();
    }
}
