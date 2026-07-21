using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General.Schema;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Collection search result.
/// </summary>
public class SearchCollection : TmdbEntity
{
    // Property to hold additional data from the JSON (populated by JSON deserialization)
#pragma warning disable CS0649 // Field is assigned by JSON deserialization
    [JsonExtensionData]
    private IDictionary<string, JsonElement>? _additionalData;
#pragma warning restore CS0649
    private string? _name;
    private string? _originalName;

    /// <summary>
    /// Gets or sets a value indicating whether the collection contains adult content.
    /// </summary>
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the collection name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name
    {
        get
        {
            // If _name is not set, attempt to retrieve the "title" property from additional data
            if (_name is null && _additionalData is not null && _additionalData.TryGetValue("title", out var nameToken))
            {
                return nameToken.ValueKind == JsonValueKind.String ? nameToken.GetString() : nameToken.ToString();
            }

            return _name;
        }
        set => _name = value;
    }

    /// <summary>
    /// Gets or sets the original language code.
    /// </summary>
    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the original collection name.
    /// </summary>
    [JsonPropertyName("original_name")]
    public string? OriginalName
    {
        get
        {
            // If _originalName is not set, attempt to retrieve the "original_title" property from additional data
            if (_originalName is null && _additionalData is not null &&
                _additionalData.TryGetValue("original_title", out var originalNameToken))
            {
                return originalNameToken.ValueKind == JsonValueKind.String ? originalNameToken.GetString() : originalNameToken.ToString();
            }

            return _originalName;
        }
        set => _originalName = value;
    }

    /// <summary>
    /// Gets or sets the overview text.
    /// </summary>
    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }
}
