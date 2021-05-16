using GavinTech.Accounts.Domain.Primitives;

namespace GavinTech.Accounts.Domain.Entities
{
    public class Account : IEntity
    {
        public Account? Parent { get; set; }
        public string Name { get; set; } = string.Empty;
        public Day? ClosedAfter { get; set; }
    }
}
