using System;
using System.IO;
using System.Text;
using System.Text.Json;
using TMDbLib.Utilities.Converters;
using TMDbLib.Utilities.JsonSerializerContexts;

namespace TMDbLib.Utilities.Serializer;

/// <summary>
/// JSON serializer implementation for TMDbLib using Newtonsoft.Json with custom converters.
/// </summary>
public class TMDbJsonSerializer : ITMDbSerializer
{
    private readonly JsonSerializerOptions _options;
    private readonly Encoding _encoding = new UTF8Encoding(false);

    private TMDbJsonSerializer()
    {
        _options = new JsonSerializerOptions()
        {
            TypeInfoResolver = TmdbJsonSerializerContext.Default,
            Converters =
            {
            }
        };
    }

    /// <summary>
    /// Gets the singleton instance of the <see cref="TMDbJsonSerializer"/>.
    /// </summary>
    public static TMDbJsonSerializer Instance { get; } = new();

    /// <summary>
    /// Serializes an object to a stream.
    /// </summary>
    /// <param name="target">The target stream to write to.</param>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="type">The type of the object.</param>
    public void Serialize<T>(Stream target, T obj)
    {
        JsonSerializer.Serialize(target, obj, _options);
    }

    /// <summary>
    /// Deserializes an object from a stream.
    /// </summary>
    /// <param name="source">The source stream to read from.</param>
    /// <param name="type">The type of the object to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    public T? Deserialize<T>(Stream source)
    {
        return JsonSerializer.Deserialize<T>(source, _options);
    }

    public object? Deserialize(Stream source, Type type)
    {
        return JsonSerializer.Deserialize(source, type, _options);
    }
}
