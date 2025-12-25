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
/// Represents a TV show with full details.
/// </summary>
public class TvShow
{
    /// <summary>
    /// Gets or sets a value indicating whether the TV show is adult content.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the account states for the TV show.
    /// </summary>
    [JsonProperty("account_states")]
    public AccountState? AccountStates { get; set; }

    /// <summary>
    /// Gets or sets the alternative titles for the TV show.
    /// </summary>
    [JsonProperty("alternative_titles")]
    public ResultContainer<AlternativeTitle>? AlternativeTitles { get; set; }

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonProperty("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the changes to the TV show.
    /// </summary>
    [JsonProperty("changes")]
    public ChangesContainer? Changes { get; set; }

    /// <summary>
    /// Gets or sets the content ratings for the TV show.
    /// </summary>
    [JsonProperty("content_ratings")]
    public ResultContainer<ContentRating>? ContentRatings { get; set; }

    /// <summary>
    /// Gets or sets the list of people who created the TV show.
    /// </summary>
    [JsonProperty("created_by")]
    public List<CreatedBy>? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the credits for the TV show.
    /// </summary>
    [JsonProperty("credits")]
    public Credits? Credits { get; set; }

    /// <summary>
    /// Gets or sets the aggregated credits for the TV show.
    /// </summary>
    [JsonProperty("aggregate_credits")]
    public CreditsAggregate? AggregateCredits { get; set; }

    /// <summary>
    /// Gets or sets the episode groups for the TV show.
    /// </summary>
    [JsonProperty("episode_groups")]
    public ResultContainer<TvGroupCollection>? EpisodeGroups { get; set; }

    /// <summary>
    /// Gets or sets the list of episode runtimes in minutes.
    /// </summary>
    [JsonProperty("episode_run_time")]
    public List<int>? EpisodeRunTime { get; set; }

    /// <summary>
    /// Gets or sets the external IDs for the TV show.
    /// </summary>
    [JsonProperty("external_ids")]
    public ExternalIdsTvShow? ExternalIds { get; set; }

    /// <summary>
    /// Gets or sets the first air date of the TV show.
    /// </summary>
    [JsonProperty("first_air_date")]
    public DateTime? FirstAirDate { get; set; }

    /// <summary>
    /// Gets or sets the list of genre IDs.
    /// </summary>
    [JsonProperty("genre_ids")]
    [JsonConverter(typeof(TmdbIntArrayAsObjectConverter)) /*#307*/]
    public List<int>? GenreIds { get; set; }

    /// <summary>
    /// Gets or sets the list of genres.
    /// </summary>
    [JsonProperty("genres")]
    public List<Genre>? Genres { get; set; }

    /// <summary>
    /// Gets or sets the homepage URL.
    /// </summary>
    [JsonProperty("homepage")]
    public string? Homepage { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the images for the TV show.
    /// </summary>
    [JsonProperty("images")]
    public Images? Images { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the TV show is still in production.
    /// </summary>
    [JsonProperty("in_production")]
    public bool InProduction { get; set; }

    /// <summary>
    /// Gets or sets the keywords associated with the TV show.
    /// </summary>
    [JsonProperty("keywords")]
    public ResultContainer<Keyword>? Keywords { get; set; }

    /// <summary>
    /// Gets or sets language ISO code ex. en.
    /// </summary>
    [JsonProperty("languages")]
    public List<string>? Languages { get; set; }

    /// <summary>
    /// Gets or sets the last air date of the TV show.
    /// </summary>
    [JsonProperty("last_air_date")]
    public DateTime? LastAirDate { get; set; }

    /// <summary>
    /// Gets or sets the last episode that aired.
    /// </summary>
    [JsonProperty("last_episode_to_air")]
    public TvEpisodeBase? LastEpisodeToAir { get; set; }

    /// <summary>
    /// Gets or sets the name of the TV show.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the next episode to air.
    /// </summary>
    [JsonProperty("next_episode_to_air")]
    public TvEpisodeBase? NextEpisodeToAir { get; set; }

    /// <summary>
    /// Gets or sets the list of networks that aired the TV show.
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
    /// Gets or sets the original name of the TV show.
    /// </summary>
    [JsonProperty("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets country ISO code ex. US.
    /// </summary>
    [JsonProperty("origin_country")]
    public List<string>? OriginCountry { get; set; }

    /// <summary>
    /// Gets or sets the overview text.
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
    /// Gets or sets the list of production companies.
    /// </summary>
    [JsonProperty("production_companies")]
    public List<ProductionCompany>? ProductionCompanies { get; set; }

    /// <summary>
    /// Gets or sets the list of production countries.
    /// </summary>
    [JsonProperty("production_countries")]
    public List<ProductionCountry>? ProductionCountries { get; set; }

    /// <summary>
    /// Gets or sets the recommendations for similar TV shows.
    /// </summary>
    [JsonProperty("recommendations")]
    public SearchContainer<SearchTv>? Recommendations { get; set; }

    /// <summary>
    /// Gets or sets the reviews for the TV show.
    /// </summary>
    [JsonProperty("reviews")]
    public SearchContainer<ReviewBase>? Reviews { get; set; }

    /// <summary>
    /// Gets or sets the list of seasons.
    /// </summary>
    [JsonProperty("seasons")]
    public List<SearchTvSeason>? Seasons { get; set; }

    /// <summary>
    /// Gets or sets similar TV shows.
    /// </summary>
    [JsonProperty("similar")]
    public SearchContainer<SearchTv>? Similar { get; set; }

    /// <summary>
    /// Gets or sets the list of spoken languages.
    /// </summary>
    [JsonProperty("spoken_languages")]
    public List<SpokenLanguage>? SpokenLanguages { get; set; }

    /// <summary>
    /// Gets or sets the status of the TV show.
    /// </summary>
    [JsonProperty("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the tagline.
    /// </summary>
    [JsonProperty("tagline")]
    public string? Tagline { get; set; }

    /// <summary>
    /// Gets or sets the translations for the TV show.
    /// </summary>
    [JsonProperty("translations")]
    public TranslationsContainer? Translations { get; set; }

    /// <summary>
    /// Gets or sets the type of the TV show.
    /// </summary>
    [JsonProperty("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the videos for the TV show.
    /// </summary>
    [JsonProperty("videos")]
    public ResultContainer<Video>? Videos { get; set; }

    /// <summary>
    /// Gets or sets the watch providers for the TV show.
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
