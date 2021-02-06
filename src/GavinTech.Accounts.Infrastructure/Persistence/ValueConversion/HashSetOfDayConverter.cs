using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using GavinTech.Accounts.Domain.Primitives;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GavinTech.Accounts.Infrastructure.Persistence.ValueConversion
{
    internal class HashSetOfDayConverter : ValueConverter<HashSet<Day>, string>
    {
        internal static HashSetOfDayConverter Instance = new HashSetOfDayConverter();

        private HashSetOfDayConverter() : base(
            set => JsonSerializer.Serialize(set, null),
            json => JsonSerializer.Deserialize<HashSet<Day>>(json, null))
        { }
    }
}
