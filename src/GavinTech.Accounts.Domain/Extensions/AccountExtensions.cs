using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Primitives;

namespace GavinTech.Accounts.Domain.Extensions;

public static class AccountExtensions
{
    public static bool IsUnderName(this Account acct, string name)
    {
        if (acct.Name == name)
        {
            return true;
        }
        if (acct.Parent == null)
        {
            return false;
        }
        return IsUnderName(acct.Parent, name);
    }

    public static Day? GetHierarchicalClosedAfter(this Account startingPoint)
    {
        Day? result = null;
        for (var acct = startingPoint; acct != null; acct = acct.Parent)
        {
            if (!acct.ClosedAfter.HasValue)
            {
                continue;
            }
            if (!result.HasValue || acct.ClosedAfter < result.Value)
            {
                result = acct.ClosedAfter;
            }
        }
        return result;
    }
}