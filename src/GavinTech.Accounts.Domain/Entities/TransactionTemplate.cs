using GavinTech.Accounts.Domain.Primitives;

namespace GavinTech.Accounts.Domain.Entities
{
    public class TransactionTemplate : EntityBase
    {
        public Day Day { get; set; }
        public Amount Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public Account Account { get; set; } = Account.Default;
    }
}
