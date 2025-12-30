using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Utilities.Converters;

internal class AccountStateConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(AccountState) ||
                objectType == typeof(TvAccountState) ||
                objectType == typeof(TvEpisodeAccountState) ||
                objectType == typeof(TvEpisodeAccountStateWithNumber);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        // Sometimes the AccountState.Rated is an object with a value in it
        // In these instances, convert it from:
        //  "rated": { "value": 5 }
        //  "rated": False
        // To:
        //  "rating": 5
        //  "rating": null

        var obj = jObject["rated"]!;
        if (obj.Type == JTokenType.Boolean)
        {
            // It's "False", so the rating is not set
            jObject.Remove("rated");
            jObject.Add("rating", null);
        }
        else if (obj.Type == JTokenType.Object)
        {
            // Read out the value
            var rating = obj["value"]?.ToObject<double>();
            jObject.Remove("rated");
            jObject.Add("rating", rating);
        }

        var result = Activator.CreateInstance(objectType);

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

        var jToken = JObject.FromObject(value);
        var ratingToken = jToken["rating"];
        jToken.Remove("rating");

        if (ratingToken is JValue obj && obj.Value is not null)
        {
            jToken["rated"] = JToken.FromObject(new { value = obj });
        }
        else
        {
            jToken["rated"] = null;
        }

        jToken.WriteTo(writer);
    }
}
