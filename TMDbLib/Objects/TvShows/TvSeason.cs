using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents a TV season with full details.
/// </summary>
public class TvSeason
{
    /// <summary>
    /// Gets or sets the account states for episodes in the season.
    /// </summary>
    [JsonPropertyName("account_states")]
    public ResultContainer<TvEpisodeAccountStateWithNumber>? AccountStates { get; set; }

    /// <summary>
    /// Gets or sets the air date of the season.
    /// </summary>
    [JsonPropertyName("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the credits for the season.
    /// </summary>
    [JsonPropertyName("credits")]
    public Credits? Credits { get; set; }

    /// <summary>
    /// Gets or sets the list of episodes in the season.
    /// </summary>
    [JsonPropertyName("episodes")]
    public List<TvSeasonEpisode>? Episodes { get; set; }

    /// <summary>
    /// Gets or sets the external IDs for the season.
    /// </summary>
    [JsonPropertyName("external_ids")]
    public ExternalIdsTvSeason? ExternalIds { get; set; }

    /// <summary>
    /// gets or sets object id, will only be populated when explicitly getting episode details.
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the poster images for the season.
    /// </summary>
    [JsonPropertyName("images")]
    public PosterImages? Images { get; set; }

    /// <summary>
    /// Gets or sets the name of the season.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the overview text of the season.
    /// </summary>
    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the season number.
    /// </summary>
    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; set; }

    /// <summary>
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the videos for the season.
    /// </summary>
    [JsonPropertyName("videos")]
    public ResultContainer<Video>? Videos { get; set; }

    /// <summary>
    /// Gets or sets the translations for the season.
    /// </summary>
    [JsonPropertyName("translations")]
    public TranslationsContainer? Translations { get; set; }
}
