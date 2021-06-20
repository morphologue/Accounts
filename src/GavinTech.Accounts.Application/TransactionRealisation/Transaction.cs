using GavinTech.Accounts.Domain.Primitives;

namespace GavinTech.Accounts.Application.TransactionRealisation
{
    public class Transaction
    {
        public Day Day { get; set; }
        public Amount Amount { get; set; }
        public Amount RunningTotal { get; set; }
        public string Description { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
    }
}
