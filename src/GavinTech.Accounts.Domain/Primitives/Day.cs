using System;
using System.Globalization;

namespace GavinTech.Accounts.Domain.Primitives
{
    public struct Day : IEquatable<Day>, IComparable, IComparable<Day>
    {
        public const string OneFormatToRuleThemAll = "yyyy'-'MM'-'dd";
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Operators
        public static bool operator ==(Day lhs, Day rhs) => lhs.Offset == rhs.Offset;
        public static bool operator !=(Day lhs, Day rhs) => lhs.Offset != rhs.Offset;
        public static bool operator <(Day lhs, Day rhs) => lhs.Offset < rhs.Offset;
        public static bool operator >(Day lhs, Day rhs) => lhs.Offset > rhs.Offset;
        public static bool operator <=(Day lhs, Day rhs) => lhs.Offset <= rhs.Offset;
        public static bool operator >=(Day lhs, Day rhs) => lhs.Offset >= rhs.Offset;

        // The sole instance member
        public int Offset { get; }

        public Day(int offset) => Offset = offset;
        public Day(DateTime dateTime) => Offset = (dateTime - Epoch).Days;
        public Day(string dayString) : this(DateTime.ParseExact(
            dayString,
            OneFormatToRuleThemAll,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal
        )) { }

        // Equality
        public override bool Equals(object? other) => Offset.Equals(ExtractOffset(other));
        public override int GetHashCode() => Offset.GetHashCode();
        public bool Equals(Day otherDay) => Offset.Equals(otherDay);

        // Comparison
        public int CompareTo(object? other) => Offset.CompareTo(ExtractOffset(other));
        public int CompareTo(Day otherDay) => Offset.CompareTo(otherDay.Offset);

        // Other object overrides
        public override string ToString() => ToDateTime().ToString(OneFormatToRuleThemAll);

        // Instance methods
        public DateTime ToDateTime() => Epoch.AddDays(Offset);

        private static int? ExtractOffset(object? other) => (other as Day?)?.Offset;
    }
}
