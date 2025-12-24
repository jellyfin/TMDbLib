using System;

namespace TMDbLib.Utilities;

/// <summary>
/// Attribute for specifying a custom string value for an enum field.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class EnumValueAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
    /// </summary>
    /// <param name="value">The custom string value for the enum field.</param>
    public EnumValueAttribute(string? value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the custom string value for the enum field.
    /// </summary>
    public string? Value { get; }
}
