using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Movie with details and related information.
/// </summary>
public class Movie
{
    /// <summary>
    /// Gets or sets the account states.
    /// </summary>
    [JsonPropertyName("account_states")]
    public AccountState? AccountStates { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the movie is adult content.
    /// </summary>
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the alternative titles.
    /// </summary>
    [JsonPropertyName("alternative_titles")]
    public AlternativeTitles? AlternativeTitles { get; set; }

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the collection this movie belongs to.
    /// </summary>
    [JsonPropertyName("belongs_to_collection")]
    public SearchCollection? BelongsToCollection { get; set; }

    /// <summary>
    /// Gets or sets the budget.
    /// </summary>
    [JsonPropertyName("budget")]
    public long Budget { get; set; }

    /// <summary>
    /// Gets or sets the change history.
    /// </summary>
    [JsonPropertyName("changes")]
    public ChangesContainer? Changes { get; set; }

    /// <summary>
    /// Gets or sets the cast and crew credits.
    /// </summary>
    [JsonPropertyName("credits")]
    public Credits? Credits { get; set; }

    /// <summary>
    /// Gets or sets the genres.
    /// </summary>
    [JsonPropertyName("genres")]
    public List<Genre>? Genres { get; set; }

    /// <summary>
    /// Gets or sets the homepage URL.
    /// </summary>
    [JsonPropertyName("homepage")]
    public string? Homepage { get; set; }

    /// <summary>
    /// Gets or sets the TMDb id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the images.
    /// </summary>
    [JsonPropertyName("images")]
    public Images? Images { get; set; }

    /// <summary>
    /// Gets or sets the IMDb id.
    /// </summary>
    [JsonPropertyName("imdb_id")]
    public string? ImdbId { get; set; }

    /// <summary>
    /// Gets or sets the keywords.
    /// </summary>
    [JsonPropertyName("keywords")]
    public KeywordsContainer? Keywords { get; set; }

    /// <summary>
    /// Gets or sets the lists containing this movie.
    /// </summary>
    [JsonPropertyName("lists")]
    public SearchContainer<ListResult>? Lists { get; set; }

    /// <summary>
    /// Gets or sets the original language code.
    /// </summary>
    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the original title.
    /// </summary>
    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; set; }

    /// <summary>
    /// Gets or sets the overview.
    /// </summary>
    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonPropertyName("popularity")]
    public double? Popularity { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the production companies.
    /// </summary>
    [JsonPropertyName("production_companies")]
    public List<ProductionCompany>? ProductionCompanies { get; set; }

    /// <summary>
    /// Gets or sets the production countries.
    /// </summary>
    [JsonPropertyName("production_countries")]
    public List<ProductionCountry>? ProductionCountries { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    [JsonPropertyName("release_date")]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the release dates by country.
    /// </summary>
    [JsonPropertyName("release_dates")]
    public ResultContainer<ReleaseDatesContainer>? ReleaseDates { get; set; }

    /// <summary>
    /// Gets or sets the external ids.
    /// </summary>
    [JsonPropertyName("external_ids")]
    public ExternalIdsMovie? ExternalIds { get; set; }

    /// <summary>
    /// Gets or sets the releases information.
    /// </summary>
    [JsonPropertyName("releases")]
    public Releases? Releases { get; set; }

    /// <summary>
    /// Gets or sets the revenue.
    /// </summary>
    [JsonPropertyName("revenue")]
    public long Revenue { get; set; }

    /// <summary>
    /// Gets or sets the user reviews.
    /// </summary>
    [JsonPropertyName("reviews")]
    public SearchContainer<ReviewBase>? Reviews { get; set; }

    /// <summary>
    /// Gets or sets the runtime in minutes.
    /// </summary>
    [JsonPropertyName("runtime")]
    public int? Runtime { get; set; }

    /// <summary>
    /// Gets or sets similar movies.
    /// </summary>
    [JsonPropertyName("similar")]
    public SearchContainer<SearchMovie>? Similar { get; set; }

    /// <summary>
    /// Gets or sets recommended movies.
    /// </summary>
    [JsonPropertyName("recommendations")]
    public SearchContainer<SearchMovie>? Recommendations { get; set; }

    /// <summary>
    /// Gets or sets the spoken languages.
    /// </summary>
    [JsonPropertyName("spoken_languages")]
    public List<SpokenLanguage>? SpokenLanguages { get; set; }

    /// <summary>
    /// Gets or sets the release status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the tagline.
    /// </summary>
    [JsonPropertyName("tagline")]
    public string? Tagline { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the translations.
    /// </summary>
    [JsonPropertyName("translations")]
    public TranslationsContainer? Translations { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the movie has video content.
    /// </summary>
    [JsonPropertyName("video")]
    public bool Video { get; set; }

    /// <summary>
    /// Gets or sets the videos.
    /// </summary>
    [JsonPropertyName("videos")]
    public ResultContainer<Video>? Videos { get; set; }

    /// <summary>
    /// Gets or sets the watch providers by country.
    /// </summary>
    [JsonPropertyName("watch/providers")]
    public SingleResultContainer<Dictionary<string, WatchProviders>>? WatchProviders { get; set; }

    /// <summary>
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the vote count.
    /// </summary>
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    /// <summary>
    /// Gets or sets the origin country ISO codes.
    /// </summary>
    [JsonPropertyName("origin_country")]
    public List<string>? OriginCountry { get; set; }
}
