using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TMDbLib.Utilities.Converters;

internal abstract class JsonCreationConverter<T> : JsonConverter
{
    protected abstract T? GetInstance(JObject jObject);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        var target = GetInstance(jObject);

        using var jsonReader = jObject.CreateReader();
        serializer.Populate(jsonReader, target!);

        return target;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        var jToken = JToken.FromObject(value);
        jToken.WriteTo(writer);
    }
}
