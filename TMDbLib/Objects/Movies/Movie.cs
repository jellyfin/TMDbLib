using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Movies
{
    public class Movie
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("imdb_id")]
        public string ImdbId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("tagline")]
        public string Tagline { get; set; }
        [JsonProperty("overview")]
        public string Overview { get; set; }
        [JsonProperty("homepage")]
        public string Homepage { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("belongs_to_collection")]
        public BelongsToCollection BelongsToCollection { get; set; }
        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }
        [JsonProperty("revenue")]
        public long Revenue { get; set; }
        [JsonProperty("budget")]
        public long Budget { get; set; }
        [JsonProperty("runtime")]
        public int? Runtime { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }
        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }
        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty("production_companies")]
        public List<ProductionCompany> ProductionCompanies { get; set; }
        [JsonProperty("production_countries")]
        public List<ProductionCountry> ProductionCountries { get; set; }
        [JsonProperty("spoken_languages")]
        public List<SpokenLanguage> SpokenLanguages { get; set; }

        [JsonProperty("alternative_titles")]
        public AlternativeTitles AlternativeTitles { get; set; }
        [JsonProperty("releases")]
        public Releases Releases { get; set; }
        [JsonProperty("credits")]
        public Credits Credits { get; set; }
        [JsonProperty("images")]
        public Images Images { get; set; }
        [JsonProperty("keywords")]
        public KeywordsContainer Keywords { get; set; }
        [JsonProperty("trailers")]
        public Trailers Trailers { get; set; }
        [JsonProperty("translations")]
        public TranslationsContainer Translations { get; set; }
        [JsonProperty("similar_movies")]
        public SearchContainer<MovieResult> SimilarMovies { get; set; }
        [JsonProperty("lists")]
        public SearchContainer<ListResult> Lists { get; set; }
        [JsonProperty("changes")]
        public List<Change> Changes { get; set; }
        [JsonProperty("account_states")]
        public MovieAccountState AccountStates { get; set; }
    }
}