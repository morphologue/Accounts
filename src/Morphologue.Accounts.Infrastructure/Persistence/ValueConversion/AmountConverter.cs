using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Infrastructure.Persistence.ValueConversion;

internal class AmountConverter : ValueConverter<Amount?, int>
{
    internal static AmountConverter Instance = new();

    private AmountConverter() : base(
        amount => amount.HasValue ? amount.Value.CentCount : default,
        centCount => new Amount(centCount))
    { }
}