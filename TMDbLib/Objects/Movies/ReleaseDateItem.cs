using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Movie release date entry.
/// </summary>
public class ReleaseDateItem
{
    /// <summary>
    /// Gets or sets the certification rating.
    /// </summary>
    [JsonPropertyName("certification")]
    public string? Certification { get; set; }

    /// <summary>
    /// Gets or sets the release descriptors.
    /// </summary>
    [JsonPropertyName("descriptors")]
    public List<string>? Descriptors { get; set; }

    /// <summary>
    /// Gets or sets the language code, e.g. en.
    /// </summary>
    [JsonPropertyName("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the release note.
    /// </summary>
    [JsonPropertyName("note")]
    public string? Note { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    [JsonPropertyName("release_date")]
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the release type.
    /// </summary>
    [JsonPropertyName("type")]
    public ReleaseDateType Type { get; set; }
}
