using System;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Credit;

/// <summary>
/// Represents a TV show season in a credit context.
/// </summary>
public class CreditSeason
{
    /// <summary>
    /// Gets or sets the air date of the season.
    /// </summary>
    [JsonPropertyName("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the poster path for the season.
    /// </summary>
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the season number.
    /// </summary>
    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; set; }
}
