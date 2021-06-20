using GavinTech.Accounts.Domain.Entities;

namespace GavinTech.Accounts.Domain.Extensions
{
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
    }
}
