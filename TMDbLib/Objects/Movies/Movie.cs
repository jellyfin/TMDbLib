using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Represents a movie with its details and related information.
/// </summary>
public class Movie
{
    /// <summary>
    /// Gets or sets the account states for this movie.
    /// </summary>
    [JsonProperty("account_states")]
    public AccountState? AccountStates { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the movie is adult content.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the alternative titles for the movie.
    /// </summary>
    [JsonProperty("alternative_titles")]
    public AlternativeTitles? AlternativeTitles { get; set; }

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonProperty("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the collection this movie belongs to.
    /// </summary>
    [JsonProperty("belongs_to_collection")]
    public SearchCollection? BelongsToCollection { get; set; }

    /// <summary>
    /// Gets or sets the movie budget.
    /// </summary>
    [JsonProperty("budget")]
    public long Budget { get; set; }

    /// <summary>
    /// Gets or sets the change history for the movie.
    /// </summary>
    [JsonProperty("changes")]
    public ChangesContainer? Changes { get; set; }

    /// <summary>
    /// Gets or sets the cast and crew credits.
    /// </summary>
    [JsonProperty("credits")]
    public Credits? Credits { get; set; }

    /// <summary>
    /// Gets or sets the list of genres for the movie.
    /// </summary>
    [JsonProperty("genres")]
    public List<Genre>? Genres { get; set; }

    /// <summary>
    /// Gets or sets the homepage URL.
    /// </summary>
    [JsonProperty("homepage")]
    public string? Homepage { get; set; }

    /// <summary>
    /// Gets or sets the TMDb movie ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the images for the movie.
    /// </summary>
    [JsonProperty("images")]
    public Images? Images { get; set; }

    /// <summary>
    /// Gets or sets the IMDb ID.
    /// </summary>
    [JsonProperty("imdb_id")]
    public string? ImdbId { get; set; }

    /// <summary>
    /// Gets or sets the keywords associated with the movie.
    /// </summary>
    [JsonProperty("keywords")]
    public KeywordsContainer? Keywords { get; set; }

    /// <summary>
    /// Gets or sets the lists containing this movie.
    /// </summary>
    [JsonProperty("lists")]
    public SearchContainer<ListResult>? Lists { get; set; }

    /// <summary>
    /// Gets or sets the original language code.
    /// </summary>
    [JsonProperty("original_language")]
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the original title.
    /// </summary>
    [JsonProperty("original_title")]
    public string? OriginalTitle { get; set; }

    /// <summary>
    /// Gets or sets the movie overview or synopsis.
    /// </summary>
    [JsonProperty("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonProperty("popularity")]
    public double? Popularity { get; set; }

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
    /// Gets or sets the release date.
    /// </summary>
    [JsonProperty("release_date")]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the release dates by country.
    /// </summary>
    [JsonProperty("release_dates")]
    public ResultContainer<ReleaseDatesContainer>? ReleaseDates { get; set; }

    /// <summary>
    /// Gets or sets the external IDs for the movie.
    /// </summary>
    [JsonProperty("external_ids")]
    public ExternalIdsMovie? ExternalIds { get; set; }

    /// <summary>
    /// Gets or sets the releases information.
    /// </summary>
    [JsonProperty("releases")]
    public Releases? Releases { get; set; }

    /// <summary>
    /// Gets or sets the movie revenue.
    /// </summary>
    [JsonProperty("revenue")]
    public long Revenue { get; set; }

    /// <summary>
    /// Gets or sets the user reviews.
    /// </summary>
    [JsonProperty("reviews")]
    public SearchContainer<ReviewBase>? Reviews { get; set; }

    /// <summary>
    /// Gets or sets the runtime in minutes.
    /// </summary>
    [JsonProperty("runtime")]
    public int? Runtime { get; set; }

    /// <summary>
    /// Gets or sets similar movies.
    /// </summary>
    [JsonProperty("similar")]
    public SearchContainer<SearchMovie>? Similar { get; set; }

    /// <summary>
    /// Gets or sets recommended movies.
    /// </summary>
    [JsonProperty("recommendations")]
    public SearchContainer<SearchMovie>? Recommendations { get; set; }

    /// <summary>
    /// Gets or sets the spoken languages in the movie.
    /// </summary>
    [JsonProperty("spoken_languages")]
    public List<SpokenLanguage>? SpokenLanguages { get; set; }

    /// <summary>
    /// Gets or sets the release status.
    /// </summary>
    [JsonProperty("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the tagline.
    /// </summary>
    [JsonProperty("tagline")]
    public string? Tagline { get; set; }

    /// <summary>
    /// Gets or sets the movie title.
    /// </summary>
    [JsonProperty("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the translations.
    /// </summary>
    [JsonProperty("translations")]
    public TranslationsContainer? Translations { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the movie has video content.
    /// </summary>
    [JsonProperty("video")]
    public bool Video { get; set; }

    /// <summary>
    /// Gets or sets the videos associated with the movie.
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
    /// Gets or sets the vote count.
    /// </summary>
    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }
}
