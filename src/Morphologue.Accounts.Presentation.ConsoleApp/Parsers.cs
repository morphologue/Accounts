using System;
using System.Linq;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Presentation.ConsoleApp;

internal static class Parsers
{
    internal static Amount ParseAmount(string amountString)
    {
        if (!amountString.Contains('.'))
        {
            amountString += ".00";
        }

        var centCount = int.Parse(amountString.Replace("$", "").Replace(".", ""));

        return new(centCount);
    }

    internal static Tuple<RecurrenceBasis, uint> ParseRecurrence(string recurringString)
    {
        var multiplicand = uint.Parse(recurringString[..^1]);
        return Tuple.Create(
            recurringString.Last() == 'm' ? RecurrenceBasis.Monthly : RecurrenceBasis.Daily,
            multiplicand);
    }
}
