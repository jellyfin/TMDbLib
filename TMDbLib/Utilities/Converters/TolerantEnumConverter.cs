using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Converter factory for enums that gracefully handles unrecognized values by falling back to defaults.
/// </summary>
public class TolerantEnumConverter : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        var type = IsNullableType(typeToConvert) ? Nullable.GetUnderlyingType(typeToConvert) : typeToConvert;
        return type is not null && type.GetTypeInfo().IsEnum;
    }

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var isNullable = IsNullableType(typeToConvert);
        var enumType = isNullable ? Nullable.GetUnderlyingType(typeToConvert)! : typeToConvert;

        var converterType = isNullable
            ? typeof(TolerantNullableEnumConverter<>).MakeGenericType(enumType)
            : typeof(TolerantEnumConverter<>).MakeGenericType(enumType);

        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }

    private static bool IsNullableType(Type t)
    {
        return t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}
