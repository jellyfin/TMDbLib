using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Movie release date entry.
/// </summary>
public class ReleaseDateItem
{
    /// <summary>
    /// Gets or sets the certification rating.
    /// </summary>
    [JsonProperty("certification")]
    public string? Certification { get; set; }

    /// <summary>
    /// Gets or sets the release descriptors.
    /// </summary>
    [JsonProperty("descriptors")]
    public List<string>? Descriptors { get; set; }

    /// <summary>
    /// Gets or sets the language code, e.g. en.
    /// </summary>
    [JsonProperty("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the release note.
    /// </summary>
    [JsonProperty("note")]
    public string? Note { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    [JsonProperty("release_date")]
    [JsonConverter(typeof(IsoDateTimeConverter))]
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the release type.
    /// </summary>
    [JsonProperty("type")]
    public ReleaseDateType Type { get; set; }
}
