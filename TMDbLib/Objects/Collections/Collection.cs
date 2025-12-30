using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.Collections;

/// <summary>
/// Represents a movie collection (e.g., a film series or franchise).
/// </summary>
public class Collection
{
    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonProperty("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the collection ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the images associated with the collection. This property is populated when the Images method is requested.
    /// </summary>
    [JsonProperty("images")]
    public Images? Images { get; set; }

    /// <summary>
    /// Gets or sets the translations for the collection. This property is populated when the Translations method is requested.
    /// </summary>
    [JsonProperty("translations")]
    public TranslationsContainer? Translations { get; set; }

    /// <summary>
    /// Gets or sets the collection name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the overview or description of the collection.
    /// </summary>
    [JsonProperty("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the list of movies that are part of this collection.
    /// </summary>
    [JsonProperty("parts")]
    public List<SearchMovie>? Parts { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonProperty("poster_path")]
    public string? PosterPath { get; set; }
}
