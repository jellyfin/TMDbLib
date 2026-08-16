using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Utilities.Serializer;

/// <summary>
/// JSON serializer implementation for TMDbLib using System.Text.Json with the
/// library's custom converters and source-generated type metadata pre-registered.
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
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,

            // Newtonsoft was case-insensitive by default; keep that behaviour so models
            // without explicit [JsonPropertyName] still bind to lowercase wire keys.
            PropertyNameCaseInsensitive = true,

            // Use the source-generated metadata so every Serialize/Deserialize call
            // resolves through pre-computed JsonTypeInfo - AOT and trimming-friendly.
            TypeInfoResolver = TMDbJsonContext.Default,
        };

        // One source-generated converter per TMDb enum - no factory, no MakeGenericType, and no
        // reflection over enum fields or their attributes.
        TmdbEnumConverters.RegisterAll(_options);

        _options.Converters.Add(new LenientDateTimeConverter());
        _options.Converters.Add(new ChangeItemConverter());
        _options.Converters.Add(new AccountStateConverter<TMDbLib.Objects.General.AccountState>(TMDbJsonContext.Default.AccountState));
        _options.Converters.Add(new AccountStateConverter<TMDbLib.Objects.TvShows.TvAccountState>(TMDbJsonContext.Default.TvAccountState));
        _options.Converters.Add(new AccountStateConverter<TMDbLib.Objects.TvShows.TvEpisodeAccountState>(TMDbJsonContext.Default.TvEpisodeAccountState));
        _options.Converters.Add(new AccountStateConverter<TMDbLib.Objects.TvShows.TvEpisodeAccountStateWithNumber>(TMDbJsonContext.Default.TvEpisodeAccountStateWithNumber));
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
        var typeInfo = _options.GetTypeInfo(type);
        JsonSerializer.Serialize(target, obj, typeInfo);
    }

    /// <inheritdoc />
    public object? Deserialize(Stream source, Type type)
    {
        var typeInfo = _options.GetTypeInfo(type);
        return JsonSerializer.Deserialize(source, typeInfo);
    }
}
