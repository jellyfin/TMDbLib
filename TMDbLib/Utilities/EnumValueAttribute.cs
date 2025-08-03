using System;

namespace TMDbLib.Utilities;

[AttributeUsage(AttributeTargets.Field)]
public sealed class EnumValueAttribute : Attribute
{
    public EnumValueAttribute(string value)
    {
        Value = value;
    }

    public string Value { get; }
}
