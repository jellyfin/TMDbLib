using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Utilities.Serializer;

/// <summary>
/// JSON serializer implementation for TMDbLib using Newtonsoft.Json with custom converters.
/// </summary>
public class TMDbJsonSerializer : ITMDbSerializer
{
    private readonly JsonSerializer _serializer;
    private readonly Encoding _encoding = new UTF8Encoding(false);

    private TMDbJsonSerializer()
    {
        _serializer = JsonSerializer.CreateDefault();
        _serializer.Converters.Add(new ChangeItemConverter());
        _serializer.Converters.Add(new AccountStateConverter());
        _serializer.Converters.Add(new KnownForConverter());
        _serializer.Converters.Add(new SearchBaseConverter());
        _serializer.Converters.Add(new TaggedImageConverter());
        _serializer.Converters.Add(new TolerantEnumConverter());
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
    public void Serialize(Stream target, object obj, Type type)
    {
        using StreamWriter sw = new StreamWriter(target, _encoding, 4096, true);
        using JsonTextWriter jw = new JsonTextWriter(sw);

        _serializer.Serialize(jw, obj, type);
    }

    /// <summary>
    /// Deserializes an object from a stream.
    /// </summary>
    /// <param name="source">The source stream to read from.</param>
    /// <param name="type">The type of the object to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    public object Deserialize(Stream source, Type type)
    {
        using StreamReader sr = new StreamReader(source, _encoding, false, 4096, true);
        using JsonTextReader jr = new JsonTextReader(sr);

        return _serializer.Deserialize(jr, type);
    }
}
