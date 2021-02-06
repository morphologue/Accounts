using GavinTech.Accounts.Domain.Primitives;

namespace GavinTech.Accounts.Domain.Entities
{
    public class Account : EntityBase
    {
        public static readonly Account Default = new();

        public Account? Parent { get; set; }
        public string Name { get; set; } = string.Empty;
        public Day? ClosedAfter { get; set; }
    }
}
