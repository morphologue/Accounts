using GavinTech.Accounts.Domain.Primitives;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GavinTech.Accounts.Infrastructure.Persistence.ValueConversion
{
    internal class AmountConverter : ValueConverter<Amount?, int>
    {
        internal static AmountConverter Instance = new AmountConverter();

        private AmountConverter() : base(
            amount => amount.Value.CentCount,
            centCount => new Amount(centCount))
        { }
    }
}
