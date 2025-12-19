using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Represents a container for release dates grouped by country.
/// </summary>
public class ReleaseDatesContainer
{
    /// <summary>
    /// Gets or sets a country code, e.g. US.
    /// </summary>
    [JsonProperty("iso_3166_1")]
    public string Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets the list of release dates for this country.
    /// </summary>
    [JsonProperty("release_dates")]
    public List<ReleaseDateItem> ReleaseDates { get; set; }
}
