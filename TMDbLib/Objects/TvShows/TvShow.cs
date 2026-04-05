using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents a TV show with full details.
/// </summary>
public class TvShow
{
    /// <summary>
    /// Gets or sets a value indicating whether the TV show is adult content.
    /// </summary>
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the account states for the TV show.
    /// </summary>
    [JsonPropertyName("account_states")]
    public AccountState? AccountStates { get; set; }

    /// <summary>
    /// Gets or sets the alternative titles for the TV show.
    /// </summary>
    [JsonPropertyName("alternative_titles")]
    public ResultContainer<AlternativeTitle>? AlternativeTitles { get; set; }

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the changes to the TV show.
    /// </summary>
    [JsonPropertyName("changes")]
    public ChangesContainer? Changes { get; set; }

    /// <summary>
    /// Gets or sets the content ratings for the TV show.
    /// </summary>
    [JsonPropertyName("content_ratings")]
    public ResultContainer<ContentRating>? ContentRatings { get; set; }

    /// <summary>
    /// Gets or sets the list of people who created the TV show.
    /// </summary>
    [JsonPropertyName("created_by")]
    public List<CreatedBy>? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the credits for the TV show.
    /// </summary>
    [JsonPropertyName("credits")]
    public Credits? Credits { get; set; }

    /// <summary>
    /// Gets or sets the aggregated credits for the TV show.
    /// </summary>
    [JsonPropertyName("aggregate_credits")]
    public CreditsAggregate? AggregateCredits { get; set; }

    /// <summary>
    /// Gets or sets the episode groups for the TV show.
    /// </summary>
    [JsonPropertyName("episode_groups")]
    public ResultContainer<TvGroupCollection>? EpisodeGroups { get; set; }

    /// <summary>
    /// Gets or sets the list of episode runtimes in minutes.
    /// </summary>
    [JsonPropertyName("episode_run_time")]
    public List<int>? EpisodeRunTime { get; set; }

    /// <summary>
    /// Gets or sets the external IDs for the TV show.
    /// </summary>
    [JsonPropertyName("external_ids")]
    public ExternalIdsTvShow? ExternalIds { get; set; }

    /// <summary>
    /// Gets or sets the first air date of the TV show.
    /// </summary>
    [JsonPropertyName("first_air_date")]
    [JsonConverter(typeof(TmdbPartialDateConverter))]
    public DateTime? FirstAirDate { get; set; }

    /// <summary>
    /// Gets or sets the list of genre IDs.
    /// </summary>
    [JsonPropertyName("genre_ids")]
    [JsonConverter(typeof(TmdbIntArrayAsObjectConverter)) /*#307*/]
    public List<int>? GenreIds { get; set; }

    /// <summary>
    /// Gets or sets the list of genres.
    /// </summary>
    [JsonPropertyName("genres")]
    public List<Genre>? Genres { get; set; }

    /// <summary>
    /// Gets or sets the homepage URL.
    /// </summary>
    [JsonPropertyName("homepage")]
    public string? Homepage { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the images for the TV show.
    /// </summary>
    [JsonPropertyName("images")]
    public Images? Images { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the TV show is still in production.
    /// </summary>
    [JsonPropertyName("in_production")]
    public bool InProduction { get; set; }

    /// <summary>
    /// Gets or sets the keywords associated with the TV show.
    /// </summary>
    [JsonPropertyName("keywords")]
    public ResultContainer<Keyword>? Keywords { get; set; }

    /// <summary>
    /// Gets or sets language ISO code ex. en.
    /// </summary>
    [JsonPropertyName("languages")]
    public List<string>? Languages { get; set; }

    /// <summary>
    /// Gets or sets the last air date of the TV show.
    /// </summary>
    [JsonPropertyName("last_air_date")]
    public DateTime? LastAirDate { get; set; }

    /// <summary>
    /// Gets or sets the last episode that aired.
    /// </summary>
    [JsonPropertyName("last_episode_to_air")]
    public TvEpisodeBase? LastEpisodeToAir { get; set; }

    /// <summary>
    /// Gets or sets the name of the TV show.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the next episode to air.
    /// </summary>
    [JsonPropertyName("next_episode_to_air")]
    public TvEpisodeBase? NextEpisodeToAir { get; set; }

    /// <summary>
    /// Gets or sets the list of networks that aired the TV show.
    /// </summary>
    [JsonPropertyName("networks")]
    public List<NetworkWithLogo>? Networks { get; set; }

    /// <summary>
    /// Gets or sets the total number of episodes.
    /// </summary>
    [JsonPropertyName("number_of_episodes")]
    [JsonConverter(typeof(TmdbNullIntAsZero))]
    public int NumberOfEpisodes { get; set; }

    /// <summary>
    /// Gets or sets the total number of seasons.
    /// </summary>
    [JsonPropertyName("number_of_seasons")]
    [JsonConverter(typeof(TmdbNullIntAsZero))]
    public int NumberOfSeasons { get; set; }

    /// <summary>
    /// Gets or sets the original language code.
    /// </summary>
    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the original name of the TV show.
    /// </summary>
    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets country ISO code ex. US.
    /// </summary>
    [JsonPropertyName("origin_country")]
    public List<string>? OriginCountry { get; set; }

    /// <summary>
    /// Gets or sets the overview text.
    /// </summary>
    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the list of production companies.
    /// </summary>
    [JsonPropertyName("production_companies")]
    public List<ProductionCompany>? ProductionCompanies { get; set; }

    /// <summary>
    /// Gets or sets the list of production countries.
    /// </summary>
    [JsonPropertyName("production_countries")]
    public List<ProductionCountry>? ProductionCountries { get; set; }

    /// <summary>
    /// Gets or sets the recommendations for similar TV shows.
    /// </summary>
    [JsonPropertyName("recommendations")]
    public SearchContainer<SearchTv>? Recommendations { get; set; }

    /// <summary>
    /// Gets or sets the reviews for the TV show.
    /// </summary>
    [JsonPropertyName("reviews")]
    public SearchContainer<ReviewBase>? Reviews { get; set; }

    /// <summary>
    /// Gets or sets the list of seasons.
    /// </summary>
    [JsonPropertyName("seasons")]
    public List<SearchTvSeason>? Seasons { get; set; }

    /// <summary>
    /// Gets or sets similar TV shows.
    /// </summary>
    [JsonPropertyName("similar")]
    public SearchContainer<SearchTv>? Similar { get; set; }

    /// <summary>
    /// Gets or sets the list of spoken languages.
    /// </summary>
    [JsonPropertyName("spoken_languages")]
    public List<SpokenLanguage>? SpokenLanguages { get; set; }

    /// <summary>
    /// Gets or sets the status of the TV show.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the tagline.
    /// </summary>
    [JsonPropertyName("tagline")]
    public string? Tagline { get; set; }

    /// <summary>
    /// Gets or sets the translations for the TV show.
    /// </summary>
    [JsonPropertyName("translations")]
    public TranslationsContainer? Translations { get; set; }

    /// <summary>
    /// Gets or sets the type of the TV show.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the videos for the TV show.
    /// </summary>
    [JsonPropertyName("videos")]
    public ResultContainer<Video>? Videos { get; set; }

    /// <summary>
    /// Gets or sets the watch providers for the TV show.
    /// </summary>
    [JsonPropertyName("watch/providers")]
    public SingleResultContainer<Dictionary<string, WatchProviders>>? WatchProviders { get; set; }

    /// <summary>
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the total vote count.
    /// </summary>
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
}
