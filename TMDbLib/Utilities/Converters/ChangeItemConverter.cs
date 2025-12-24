using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.Changes;

namespace TMDbLib.Utilities.Converters;

internal class ChangeItemConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ChangeItemBase);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        ChangeItemBase? result;
        if (jObject["action"] is null)
        {
            // We cannot determine the correct type, let's hope we were provided one
            var instance = Activator.CreateInstance(objectType);
            result = instance as ChangeItemBase;
        }
        else
        {
            // Determine the type based on the media_type
            var mediaType = jObject["action"]?.ToObject<ChangeAction>();

            switch (mediaType)
            {
                case ChangeAction.Added:
                    result = new ChangeItemAdded();
                    break;
                case ChangeAction.Created:
                    result = new ChangeItemCreated();
                    break;
                case ChangeAction.Updated:
                    result = new ChangeItemUpdated();
                    break;
                case ChangeAction.Deleted:
                    result = new ChangeItemDeleted();
                    break;
                case ChangeAction.Destroyed:
                    result = new ChangeItemDestroyed();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Populate the result
        if (result is not null)
        {
            using var jsonReader = jObject.CreateReader();
            serializer.Populate(jsonReader, result);
        }

        return result;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        var jToken = JToken.FromObject(value);
        serializer.Serialize(writer, jToken);
    }
}
