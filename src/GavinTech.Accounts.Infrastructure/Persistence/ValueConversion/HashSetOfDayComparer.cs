using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using GavinTech.Accounts.Domain.Primitives;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GavinTech.Accounts.Infrastructure.Persistence.ValueConversion
{
    internal class HashSetOfDayComparer : ValueComparer<HashSet<Day>>
    {
        internal static HashSetOfDayComparer Instance = new HashSetOfDayComparer();

        internal ValueComparer<HashSet<Day>> Comparer { get; }

        private HashSetOfDayComparer() : base(
            (set1, set2) => set1.SequenceEqual(set2),
            set => JsonSerializer.Serialize(set, null).GetHashCode(),
            set => set.ToHashSet())
        { }
    }
}
