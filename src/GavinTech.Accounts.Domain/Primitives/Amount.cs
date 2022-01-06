using System;

namespace GavinTech.Accounts.Domain.Primitives;

public struct Amount : IEquatable<Amount>, IComparable, IComparable<Amount>
{
    // Operators
    public static bool operator ==(Amount lhs, Amount rhs) => lhs.CentCount == rhs.CentCount;
    public static bool operator !=(Amount lhs, Amount rhs) => lhs.CentCount != rhs.CentCount;
    public static bool operator <(Amount lhs, Amount rhs) => lhs.CentCount < rhs.CentCount;
    public static bool operator >(Amount lhs, Amount rhs) => lhs.CentCount > rhs.CentCount;
    public static bool operator <=(Amount lhs, Amount rhs) => lhs.CentCount <= rhs.CentCount;
    public static bool operator >=(Amount lhs, Amount rhs) => lhs.CentCount >= rhs.CentCount;
    public static Amount operator +(Amount lhs, Amount rhs) => new(lhs.CentCount + rhs.CentCount);

    // The sole instance member
    public int CentCount { get; }

    public Amount(int centCount) => CentCount = centCount;

    // Equality
    public override bool Equals(object? other) => CentCount.Equals(ExtractCentCount(other));
    public override int GetHashCode() => CentCount;
    public bool Equals(Amount otherAmount) => CentCount.Equals(otherAmount.CentCount);

    // Comparison
    public int CompareTo(object? other) => CentCount.CompareTo(ExtractCentCount(other));
    public int CompareTo(Amount otherAmount) => CentCount.CompareTo(otherAmount.CentCount);

    // Other object overrides
    public override string ToString()
    {
        var abs = Math.Abs(CentCount);
        return $"{(CentCount < 0 ? "-" : string.Empty)}{abs / 100}.{abs % 100:00}";
    }

    // Instance methods
    public decimal ToDecimal() => CentCount / 100m;

    private static int? ExtractCentCount(object? other) => (other as Amount?)?.CentCount;
}