using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Domain.Extensions;

public static class NullableExtensions
{
    public static Day? ToDay(string? strung) => strung == null ? null : new Day(strung);
}