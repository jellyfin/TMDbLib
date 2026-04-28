using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TMDbLib.Utilities.Converters;
using TMDbLib.Utilities.JsonSerializerContexts;

namespace TMDbLib.Utilities.Serializer;

/// <summary>
/// JSON serializer implementation for TMDbLib using Newtonsoft.Json with custom converters.
/// </summary>
public class TMDbJsonSerializer : ITMDbSerializer
{
    private readonly Encoding _encoding = new UTF8Encoding(false);

    private TMDbJsonSerializer()
    {
    }

    /// <summary>
    /// Gets the singleton instance of the <see cref="TMDbJsonSerializer"/>.
    /// </summary>
    public static TMDbJsonSerializer Instance { get; } = new();

    /// <inheritdoc/>
    public void Serialize<T>(Stream target, T obj)
    {
        JsonSerializer.Serialize(target, obj, GetTypeInfo<T>());
    }

    /// <inheritdoc/>
    public T? Deserialize<T>(Stream source)
    {
        return JsonSerializer.Deserialize(source, GetTypeInfo<T>());
    }

    /// <inheritdoc/>
    public object? Deserialize(Stream source, Type type)
    {
        return JsonSerializer.Deserialize(source, type, TmdbJsonSerializerContext.Default);
    }

    private JsonTypeInfo<T> GetTypeInfo<T>()
    {
        return (JsonTypeInfo<T>?)TmdbJsonSerializerContext.Default.GetTypeInfo(typeof(T))
               ?? throw new NotSupportedException($"Type '{typeof(T).FullName}' is not registered in {nameof(TmdbJsonSerializerContext)}.");
    }
}
