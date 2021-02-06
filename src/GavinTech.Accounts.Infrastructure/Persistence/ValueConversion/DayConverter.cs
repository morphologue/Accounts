using System;
using GavinTech.Accounts.Domain.Primitives;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GavinTech.Accounts.Infrastructure.Persistence.ValueConversion
{
    internal class DayConverter : ValueConverter<Day?, int>
    {
        internal static DayConverter Instance = new DayConverter();

        private DayConverter() : base(
            day => day.Value.Offset,
            offset => new Day(offset))
        { }
    }
}
