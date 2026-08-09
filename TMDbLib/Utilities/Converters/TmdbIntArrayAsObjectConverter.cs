using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// In some cases, TMDb sends a list of integers as an object.
/// </summary>
internal class TmdbIntArrayAsObjectConverter : JsonConverter<List<int>?>
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

        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            // Read the array by hand rather than recursing into JsonSerializer - the reflection-based
            // overloads are not AOT-safe, and List<int> is not registered in TMDbJsonContext.
            var values = new List<int>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType != JsonTokenType.Number)
                {
                    throw new JsonException("Unable to convert list of integers");
                }

                values.Add(reader.GetInt32());
            }

            return values;
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            reader.Skip();
            return new List<int>();
        }

        throw new InvalidOperationException("Unable to convert list of integers");
    }

    public override void Write(Utf8JsonWriter writer, List<int>? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();

        foreach (var item in value)
        {
            writer.WriteNumberValue(item);
        }

        writer.WriteEndArray();
    }
}
