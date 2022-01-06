using System;

namespace GavinTech.Accounts.Domain.Utilities;

/// <summary>A box which unlike Nullable allows <see cref="T"/> to be nullable. Useful for requests
/// in which a value can optionally be updated to null.</summary>
public struct PatchBox<T>
{
    public bool IsSpecified { get; init; }

    private T _value;
    public T Value
    {
        get => IsSpecified ? _value : throw new InvalidOperationException("Value has not been specified");
        init => _value = value;
    }
}