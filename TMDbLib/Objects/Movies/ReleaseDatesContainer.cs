using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Release dates grouped by country.
/// </summary>
public class ReleaseDatesContainer
{
    /// <summary>
    /// Gets or sets the country code, e.g. US.
    /// </summary>
    [JsonProperty("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets the release dates for this country.
    /// </summary>
    [JsonProperty("release_dates")]
    public List<ReleaseDateItem>? ReleaseDates { get; set; }
}
