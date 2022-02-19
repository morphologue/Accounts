using GavinTech.Accounts.Domain.Primitives;

namespace GavinTech.Accounts.Domain.Entities;

public class Account : IEntity
{
    public const string RootAccountName = "[Root]";

    public Account? Parent { get; set; }
    public string Name { get; set; } = RootAccountName;
    public Day? ClosedAfter { get; set; }
}
