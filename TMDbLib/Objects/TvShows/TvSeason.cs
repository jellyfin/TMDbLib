using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// TV season with full details.
/// </summary>
public class TvSeason
{
    /// <summary>
    /// Gets or sets the MongoDB-style object id returned by TMDb (the "_id" field).
    /// </summary>
    [JsonProperty("_id")]
    public string? UnderscoreId { get; set; }

    /// <summary>
    /// Gets or sets the account states for episodes in the season.
    /// </summary>
    [JsonProperty("account_states")]
    public ResultContainer<TvEpisodeAccountStateWithNumber>? AccountStates { get; set; }

    /// <summary>
    /// Gets or sets the air date.
    /// </summary>
    [JsonProperty("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the credits.
    /// </summary>
    [JsonProperty("credits")]
    public Credits? Credits { get; set; }

    /// <summary>
    /// Gets or sets the aggregated credits.
    /// </summary>
    [JsonProperty("aggregate_credits")]
    public CreditsAggregate? AggregateCredits { get; set; }

    /// <summary>
    /// Gets or sets the episodes.
    /// </summary>
    [JsonProperty("episodes")]
    public List<TvSeasonEpisode>? Episodes { get; set; }

    /// <summary>
    /// Gets or sets the external ids.
    /// </summary>
    [JsonProperty("external_ids")]
    public ExternalIdsTvSeason? ExternalIds { get; set; }

    /// <summary>
    /// Gets or sets the season id; populated only when explicitly getting episode details.
    /// </summary>
    [JsonProperty("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the poster images.
    /// </summary>
    [JsonProperty("images")]
    public PosterImages? Images { get; set; }

    /// <summary>
    /// Gets or sets the season name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the networks the season aired on.
    /// </summary>
    [JsonProperty("networks")]
    public List<NetworkWithLogo>? Networks { get; set; }

    /// <summary>
    /// Gets or sets the overview.
    /// </summary>
    [JsonProperty("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonProperty("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the season number.
    /// </summary>
    [JsonProperty("season_number")]
    public int SeasonNumber { get; set; }

    /// <summary>
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the videos.
    /// </summary>
    [JsonProperty("videos")]
    public ResultContainer<Video>? Videos { get; set; }

    /// <summary>
    /// Gets or sets the translations.
    /// </summary>
    [JsonProperty("translations")]
    public TranslationsContainer? Translations { get; set; }
}
