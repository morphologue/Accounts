using GavinTech.Accounts.Domain.Primitives;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GavinTech.Accounts.Infrastructure.Persistence.ValueConversion
{
    internal class HashSetOfDayComparer : ValueComparer<HashSet<Day>>
    {
        internal static readonly HashSetOfDayComparer Instance = new();

        private HashSetOfDayComparer() : base(
            (set1, set2) => set1!.OrderBy(d => d.Offset).SequenceEqual(set2!.OrderBy(d => d.Offset)),
            set => JsonSerializer.Serialize(set.OrderBy(d => d.Offset).Select(d => d.Offset), (JsonSerializerOptions?)null)
                .GetHashCode(),
            set => set.ToHashSet())
        { }
    }
}
