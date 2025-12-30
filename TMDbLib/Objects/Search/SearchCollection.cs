using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a collection search result.
/// </summary>
public class SearchCollection : SearchBase
{
    // Property to hold additional data from the JSON (populated by JSON deserialization)
#pragma warning disable CS0649 // Field is assigned by JSON deserialization
    [JsonExtensionData]
    private IDictionary<string, JToken>? _additionalData;
#pragma warning restore CS0649
    private string? _name;
    private string? _originalName;

    /// <summary>
    /// Gets or sets a value indicating whether the collection contains adult content.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonProperty("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the name of the collection.
    /// </summary>
    [JsonProperty("name")]
    public string? Name
    {
        get
        {
            // If _name is not set, attempt to retrieve the "title" property from additional data
            if (_name is null && _additionalData is not null && _additionalData.TryGetValue("title", out var nameToken))
            {
                return nameToken.ToString();
            }

            return _name;
        }
        set => _name = value;
    }

    /// <summary>
    /// Gets or sets the original language code.
    /// </summary>
    [JsonProperty("original_language")]
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the original name of the collection.
    /// </summary>
    [JsonProperty("original_name")]
    public string? OriginalName
    {
        get
        {
            // If _originalName is not set, attempt to retrieve the "original_title" property from additional data
            if (_originalName is null && _additionalData is not null &&
                _additionalData.TryGetValue("original_title", out var originalNameToken))
            {
                return originalNameToken.ToString();
            }

            return _originalName;
        }
        set => _originalName = value;
    }

    /// <summary>
    /// Gets or sets the overview text of the collection.
    /// </summary>
    [JsonProperty("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonProperty("poster_path")]
    public string? PosterPath { get; set; }
}
