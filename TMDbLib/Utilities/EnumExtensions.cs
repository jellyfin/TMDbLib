using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        where T : struct
    {
        var type = enumerationValue.GetType();
        var typeInfo = type.GetTypeInfo();

        if (!typeInfo.IsEnum)
        {
            throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
        }

        var members = typeof(T).GetTypeInfo().DeclaredMembers;

        var requestedName = enumerationValue.ToString();

        // Tries to find a DisplayAttribute for a potential friendly name for the enum
        foreach (var member in members)
        {
            if (member.Name != requestedName)
            {
                continue;
            }

            foreach (var attributeData in member.CustomAttributes)
            {
                if (attributeData.AttributeType != typeof(EnumValueAttribute))
                {
                    continue;
                }

                // Pull out the Value
                if (!attributeData.ConstructorArguments.Any())
                {
                    break;
                }

                var argument = attributeData.ConstructorArguments.First();

                if (argument.Value is string stringValue)
                {
                    return stringValue;
                }

                break;
            }

            break;
        }

        // If we have no description attribute, just return the ToString of the enum
        return requestedName ?? string.Empty;
    }
}
