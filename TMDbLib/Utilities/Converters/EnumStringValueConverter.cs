using System;
using System.Reflection;
using Newtonsoft.Json;

namespace TMDbLib.Utilities.Converters;

internal class EnumStringValueConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        var str = EnumMemberCache.GetString(value);
        writer.WriteValue(str);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var val = EnumMemberCache.GetValue(reader.Value as string, objectType);

        return val;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.GetTypeInfo().IsEnum;
    }
}
