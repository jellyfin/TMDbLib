using System;
using System.IO;
using System.Text.Json;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Utilities.Serializer;

/// <summary>
/// JSON serializer implementation for TMDbLib using System.Text.Json with the
/// library's custom converters pre-registered.
/// </summary>
public class TMDbJsonSerializer : ITMDbSerializer
{
    private readonly JsonSerializerOptions _options;

    private TMDbJsonSerializer()
    {
        _options = new JsonSerializerOptions
        {
            // TMDb returns nulls in many places; we just want to skip writing them
            // back when serialising. Reads ignore the value either way.
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,

            // Newtonsoft was case-insensitive by default; keep that behaviour so models
            // without explicit [JsonPropertyName] still bind to lowercase wire keys.
            PropertyNameCaseInsensitive = true,
        };

        // Order matters: list-level converters are applied via property attributes,
        // global converters here cover the rest.
        _options.Converters.Add(new TolerantEnumConverter());
        _options.Converters.Add(new EnumStringValueConverter());
        _options.Converters.Add(new LenientDateTimeConverter());
        _options.Converters.Add(new ChangeItemConverter());
        _options.Converters.Add(new AccountStateConverter());
        _options.Converters.Add(new TaggedImageConverter());
        _options.Converters.Add(new TmdbEntityConverter());
    }

    /// <summary>
    /// Gets the singleton instance of the <see cref="TMDbJsonSerializer"/>.
    /// </summary>
    public static TMDbJsonSerializer Instance { get; } = new();

    /// <summary>
    /// Gets the <see cref="JsonSerializerOptions"/> in use.
    /// </summary>
    public JsonSerializerOptions Options => _options;

    /// <inheritdoc />
    public void Serialize(Stream target, object obj, Type type)
    {
        JsonSerializer.Serialize(target, obj, type, _options);
    }

    /// <inheritdoc />
    public object? Deserialize(Stream source, Type type)
    {
        return JsonSerializer.Deserialize(source, type, _options);
    }
}
