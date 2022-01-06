using GavinTech.Accounts.Domain.Primitives;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GavinTech.Accounts.Infrastructure.Persistence.ValueConversion;

internal class HashSetOfDayConverter : ValueConverter<HashSet<Day>, string>
{
    internal static HashSetOfDayConverter Instance = new();

    private HashSetOfDayConverter() : base(
        set => JsonSerializer.Serialize(set.Select(d => d.Offset), (JsonSerializerOptions?)null),
        json => (JsonSerializer.Deserialize<List<int>>(json, (JsonSerializerOptions?)null) ?? Enumerable.Empty<int>())
            .Select(f => new Day(f))
            .ToHashSet())
    { }
}