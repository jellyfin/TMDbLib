using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter for enum values that gracefully handles unrecognized values by falling back to defaults.
/// </summary>
public class TolerantEnumConverter : JsonConverter
{
    /// <inheritdoc />
    public override bool CanConvert(Type objectType)
    {
        var type = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
        return type is not null && type.GetTypeInfo().IsEnum;
    }

    /// <inheritdoc />
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var isNullable = IsNullableType(objectType);
        var enumType = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;

        if (enumType is null)
        {
            return null;
        }

        var names = Enum.GetNames(enumType);

        if (reader.TokenType == JsonToken.String)
        {
            var enumText = reader.Value?.ToString();

            if (!string.IsNullOrEmpty(enumText))
            {
                var match = names.FirstOrDefault(n => string.Equals(n, enumText, StringComparison.OrdinalIgnoreCase));

                if (match is not null)
                {
                    return Enum.Parse(enumType, match);
                }
            }
        }
        else if (reader.TokenType == JsonToken.Integer)
        {
            var enumVal = Convert.ToInt32(reader.Value, CultureInfo.InvariantCulture);
            var values = (int[])Enum.GetValues(enumType);
            if (values.Contains(enumVal))
            {
                return Enum.Parse(enumType, enumVal.ToString(CultureInfo.InvariantCulture));
            }
        }

        if (!isNullable)
        {
            var defaultName = names.FirstOrDefault(n => string.Equals(n, "Unknown", StringComparison.OrdinalIgnoreCase)) ?? names.First();

            return Enum.Parse(enumType, defaultName);
        }

        return null;
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteValue(value.ToString());
    }

    private static bool IsNullableType(Type t)
    {
        return t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}
