using System;
using System.Linq;
using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Primitives;

namespace GavinTech.Accounts.Presentation.ConsoleApp;

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
