using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Helpers;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.TvShows
{
    public class TvShow
    {
        [JsonProperty("account_states")]
        public AccountState AccountStates { get; set; }

        [JsonProperty("alternative_titles")]
        public ResultContainer<AlternativeTitle> AlternativeTitles { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("changes")]
        public ChangesContainer Changes { get; set; }

        [JsonProperty("content_ratings")]
        public ResultContainer<ContentRating> ContentRatings { get; set; }

        [JsonProperty("created_by")]
        public List<CreatedBy> CreatedBy { get; set; }

        [JsonProperty("credits")]
        public Credits Credits { get; set; }

        [JsonProperty("episode_run_time")]
        public List<int> EpisodeRunTime { get; set; }

        [JsonProperty("external_ids")]
        public ExternalIdsTvShow ExternalIds { get; set; }

        [JsonProperty("first_air_date")]
        public DateTime? FirstAirDate { get; set; }

        [JsonProperty("genre_ids")]
        public List<int> GenreIds { get; set; }

        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; }

        [JsonProperty("homepage")]
        public string Homepage { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("images")]
        public Images Images { get; set; }

        [JsonProperty("in_production")]
        public bool InProduction { get; set; }

        [JsonProperty("keywords")]
        public ResultContainer<Keyword> Keywords { get; set; }

        /// <summary>
        /// language ISO code ex. en
        /// </summary>
        [JsonProperty("languages")]
        public List<string> Languages { get; set; }

        [JsonProperty("last_air_date")]
        public DateTime? LastAirDate { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("networks")]
        public List<Network> Networks { get; set; }

        [JsonProperty("number_of_episodes")]
        [JsonConverter(typeof(TmdbNullIntAsZero))]
        public int NumberOfEpisodes { get; set; }

        [JsonProperty("number_of_seasons")]
        [JsonConverter(typeof(TmdbNullIntAsZero))]
        public int NumberOfSeasons { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonProperty("original_name")]
        public string OriginalName { get; set; }

        /// <summary>
        /// Country ISO code ex. US
        /// </summary>
        [JsonProperty("origin_country")]
        public List<string> OriginCountry { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("production_companies")]
        public List<ProductionCompany> ProductionCompanies { get; set; }

        [JsonProperty("seasons")]
        public List<SearchTvSeason> Seasons { get; set; }

        [JsonProperty("similar")]
        public ResultContainer<TvShow> Similar { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("translations")]
        public TranslationsContainer Translations { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("videos")]
        public ResultContainer<Video> Videos { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }
    }
}
