using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Utilities.JsonSerializerContexts;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// In some cases, TMDb sends a list of integers as an object.
/// </summary>
internal class TmdbIntArrayAsObjectConverter : JsonConverter<List<int>>
{
    public override List<int>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Sometimes the genre_ids is an empty object, instead of an array
        // In these instances, convert it from:
        //  "genre_ids": {}
        //  "genre_ids": [ 1 ]
        // To:
        //  "genre_ids": []
        //  "genre_ids": [ 1 ]

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return JsonSerializer.Deserialize(ref reader, TmdbJsonSerializerContext.Default.ListInt32);
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            reader.Skip();
            return [];
        }

        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        throw new InvalidOperationException("Unable to convert list of integers");
    }

    public override void Write(Utf8JsonWriter writer, List<int> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, TmdbJsonSerializerContext.Default.ListInt32);
    }
}
