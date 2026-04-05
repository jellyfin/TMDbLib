using System;
using System.Linq;
using System.Reflection;
using TMDbLib.Utilities.JsonSerializerContexts;

namespace TMDbLib.Utilities;

/// <summary>
/// Extension methods for enum types.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the description of an enum value from its <see cref="EnumValueAttribute"/>, or the enum name if no attribute is present.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="enumerationValue">The enum value.</param>
    /// <returns>The description string from the attribute, or the enum value name.</returns>
    public static string GetDescription<T>(this T enumerationValue)
        where T : struct, Enum
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
        }

        var field = typeof(T).GetField(enumerationValue.ToString());

        var attributeData = field?.GetCustomAttribute<EnumValueAttribute>();
        // If we have no description attribute, just return the ToString of the enum
        return attributeData?.Value ?? enumerationValue.ToString();
    }
}
