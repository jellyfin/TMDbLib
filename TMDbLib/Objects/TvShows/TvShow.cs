using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// TV show with full details.
/// </summary>
public class TvShow
{
    /// <summary>
    /// Gets or sets a value indicating whether the TV show is adult content.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the account states.
    /// </summary>
    [JsonProperty("account_states")]
    public AccountState? AccountStates { get; set; }

    /// <summary>
    /// Gets or sets the alternative titles.
    /// </summary>
    [JsonProperty("alternative_titles")]
    public ResultContainer<AlternativeTitle>? AlternativeTitles { get; set; }

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonProperty("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the change history.
    /// </summary>
    [JsonProperty("changes")]
    public ChangesContainer? Changes { get; set; }

    /// <summary>
    /// Gets or sets the content ratings.
    /// </summary>
    [JsonProperty("content_ratings")]
    public ResultContainer<ContentRating>? ContentRatings { get; set; }

    /// <summary>
    /// Gets or sets the creators.
    /// </summary>
    [JsonProperty("created_by")]
    public List<CreatedBy>? CreatedBy { get; set; }

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
    /// Gets or sets the episode groups.
    /// </summary>
    [JsonProperty("episode_groups")]
    public ResultContainer<TvGroupCollection>? EpisodeGroups { get; set; }

    /// <summary>
    /// Gets or sets the episode runtimes in minutes.
    /// </summary>
    [JsonProperty("episode_run_time")]
    public List<int>? EpisodeRunTime { get; set; }

    /// <summary>
    /// Gets or sets the external ids.
    /// </summary>
    [JsonProperty("external_ids")]
    public ExternalIdsTvShow? ExternalIds { get; set; }

    /// <summary>
    /// Gets or sets the first air date.
    /// </summary>
    [JsonProperty("first_air_date")]
    public DateTime? FirstAirDate { get; set; }

    /// <summary>
    /// Gets or sets the genre ids.
    /// </summary>
    [JsonProperty("genre_ids")]
    [JsonConverter(typeof(TmdbIntArrayAsObjectConverter)) /*#307*/]
    public List<int>? GenreIds { get; set; }

    /// <summary>
    /// Gets or sets the genres.
    /// </summary>
    [JsonProperty("genres")]
    public List<Genre>? Genres { get; set; }

    /// <summary>
    /// Gets or sets the homepage URL.
    /// </summary>
    [JsonProperty("homepage")]
    public string? Homepage { get; set; }

    /// <summary>
    /// Gets or sets the TMDb id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the images.
    /// </summary>
    [JsonProperty("images")]
    public Images? Images { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the TV show is still in production.
    /// </summary>
    [JsonProperty("in_production")]
    public bool InProduction { get; set; }

    /// <summary>
    /// Gets or sets the keywords.
    /// </summary>
    [JsonProperty("keywords")]
    public ResultContainer<Keyword>? Keywords { get; set; }

    /// <summary>
    /// Gets or sets the language ISO codes, e.g. en.
    /// </summary>
    [JsonProperty("languages")]
    public List<string>? Languages { get; set; }

    /// <summary>
    /// Gets or sets the lists containing this TV show; populated when the Lists method is requested.
    /// </summary>
    [JsonProperty("lists")]
    public SearchContainer<Movies.ListResult>? Lists { get; set; }

    /// <summary>
    /// Gets or sets the last air date.
    /// </summary>
    [JsonProperty("last_air_date")]
    public DateTime? LastAirDate { get; set; }

    /// <summary>
    /// Gets or sets the last episode that aired.
    /// </summary>
    [JsonProperty("last_episode_to_air")]
    public TvEpisodeBase? LastEpisodeToAir { get; set; }

    /// <summary>
    /// Gets or sets the TV show name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the next episode to air.
    /// </summary>
    [JsonProperty("next_episode_to_air")]
    public TvEpisodeBase? NextEpisodeToAir { get; set; }

    /// <summary>
    /// Gets or sets the networks that aired the TV show.
    /// </summary>
    [JsonProperty("networks")]
    public List<NetworkWithLogo>? Networks { get; set; }

    /// <summary>
    /// Gets or sets the total number of episodes.
    /// </summary>
    [JsonProperty("number_of_episodes")]
    [JsonConverter(typeof(TmdbNullIntAsZero))]
    public int NumberOfEpisodes { get; set; }

    /// <summary>
    /// Gets or sets the total number of seasons.
    /// </summary>
    [JsonProperty("number_of_seasons")]
    [JsonConverter(typeof(TmdbNullIntAsZero))]
    public int NumberOfSeasons { get; set; }

    /// <summary>
    /// Gets or sets the original language code.
    /// </summary>
    [JsonProperty("original_language")]
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the original name.
    /// </summary>
    [JsonProperty("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the origin country ISO codes, e.g. US.
    /// </summary>
    [JsonProperty("origin_country")]
    public List<string>? OriginCountry { get; set; }

    /// <summary>
    /// Gets or sets the overview.
    /// </summary>
    [JsonProperty("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonProperty("popularity")]
    public double Popularity { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonProperty("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the production companies.
    /// </summary>
    [JsonProperty("production_companies")]
    public List<ProductionCompany>? ProductionCompanies { get; set; }

    /// <summary>
    /// Gets or sets the production countries.
    /// </summary>
    [JsonProperty("production_countries")]
    public List<ProductionCountry>? ProductionCountries { get; set; }

    /// <summary>
    /// Gets or sets the recommendations.
    /// </summary>
    [JsonProperty("recommendations")]
    public SearchContainer<SearchTv>? Recommendations { get; set; }

    /// <summary>
    /// Gets or sets the reviews.
    /// </summary>
    [JsonProperty("reviews")]
    public SearchContainer<ReviewBase>? Reviews { get; set; }

    /// <summary>
    /// Gets or sets the seasons.
    /// </summary>
    [JsonProperty("seasons")]
    public List<SearchTvSeason>? Seasons { get; set; }

    /// <summary>
    /// Gets or sets similar TV shows.
    /// </summary>
    [JsonProperty("similar")]
    public SearchContainer<SearchTv>? Similar { get; set; }

    /// <summary>
    /// Gets or sets the spoken languages.
    /// </summary>
    [JsonProperty("spoken_languages")]
    public List<SpokenLanguage>? SpokenLanguages { get; set; }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    [JsonProperty("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the tagline.
    /// </summary>
    [JsonProperty("tagline")]
    public string? Tagline { get; set; }

    /// <summary>
    /// Gets or sets the translations.
    /// </summary>
    [JsonProperty("translations")]
    public TranslationsContainer? Translations { get; set; }

    /// <summary>
    /// Gets or sets the TV show type.
    /// </summary>
    [JsonProperty("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the videos.
    /// </summary>
    [JsonProperty("videos")]
    public ResultContainer<Video>? Videos { get; set; }

    /// <summary>
    /// Gets or sets the watch providers by country.
    /// </summary>
    [JsonProperty("watch/providers")]
    public SingleResultContainer<Dictionary<string, WatchProviders>>? WatchProviders { get; set; }

    /// <summary>
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the total vote count.
    /// </summary>
    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }
}
