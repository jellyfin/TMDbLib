using System;

namespace TMDbLib.Utilities;

/// <summary>
/// Marks an enum for source generation of its TMDb string mapping. The generator emits a
/// <c>GetDescription()</c> extension method from the members' <see cref="EnumValueAttribute"/>
/// values and, unless <see cref="GenerateJsonConverter"/> is disabled, a matching
/// <c>JsonConverter</c>. This replaces runtime reflection over enum fields.
/// </summary>
[AttributeUsage(AttributeTargets.Enum)]
public sealed class TolerantEnumAttribute : Attribute
{
    /// <summary>
    /// Gets or sets a value indicating whether a <c>JsonConverter</c> is generated for this enum.
    /// Set to <c>false</c> for enums that only appear in URLs and are never serialized.
    /// </summary>
    public bool GenerateJsonConverter { get; set; } = true;
}
