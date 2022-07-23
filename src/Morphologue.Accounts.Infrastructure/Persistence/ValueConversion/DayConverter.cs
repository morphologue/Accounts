using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Infrastructure.Persistence.ValueConversion;

internal class DayConverter : ValueConverter<Day?, int>
{
    internal static DayConverter Instance = new();

    private DayConverter() : base(
        day => day.HasValue ? day.Value.Offset : default,
        offset => new Day(offset))
    { }
}