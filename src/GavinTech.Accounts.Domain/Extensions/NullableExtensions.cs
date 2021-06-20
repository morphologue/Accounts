using GavinTech.Accounts.Domain.Primitives;

namespace GavinTech.Accounts.Domain.Extensions
{
    public static class NullableExtensions
    {
        public static Day? ToDay(string? strung) => strung == null ? null : new Day(strung);
    }
}
