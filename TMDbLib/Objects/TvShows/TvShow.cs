using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.TvShows
{
    public class TvShow : SearchTv
    {
        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("episode_run_time")]
        public List<int> EpisodeRunTime { get; set; }

        [JsonProperty("homepage")]
        public string Homepage { get; set; }


        [JsonProperty("last_air_date")]
        public DateTime? LastAirDate { get; set; }


        [JsonProperty("number_of_seasons")]
        public int NumberOfSeasons { get; set; }

        [JsonProperty("number_of_episodes")]
        public int NumberOfEpisodes { get; set; }

        [JsonProperty("seasons")]
        public List<TvSeason> Seasons { get; set; }


        [JsonProperty("in_production")]
        public bool InProduction { get; set; }

        [JsonProperty("production_companies")]
        public List<ProductionCompany> ProductionCompanies { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }


        [JsonProperty("created_by")]
        public List<Person> CreatedBy { get; set; }

        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; }

        /// <summary>
        /// language ISO code ex. en
        /// </summary>
        [JsonProperty("languages")]
        public List<string> Languages { get; set; }

        [JsonProperty("networks")]
        public List<Network> Networks { get; set; }

        /// <summary>
        /// Country ISO code ex. US
        /// </summary>
        [JsonProperty("origin_country")]
        public List<string> OriginCountry { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }


        [JsonProperty("images")]
        public Images Images { get; set; }

        [JsonProperty("credits")]
        public Credits Credits { get; set; }

        [JsonProperty("external_ids")]
        public ExternalIds ExternalIds { get; set; }

        [JsonProperty("videos")]
        public ResultContainer<Video> Videos { get; set; }

        [JsonProperty("content_ratings")]
        public ResultContainer<ContentRating> ContentRatings { get; set; }

        [JsonProperty("alternative_titles")]
        public ResultContainer<AlternativeTitle> AlternativeTitles { get; set; }

        [JsonProperty("keywords")]
        public ResultContainer<Keyword> Keywords { get; set; }

        [JsonProperty("similar")]
        public ResultContainer<TvShow> Similar { get; set; }

        [JsonProperty("changes")]
        public ChangesContainer Changes { get; set; }

        [JsonProperty("translations")]
        public TranslationsContainer Translations { get; set; }

        [JsonProperty("account_states")]
        public AccountState AccountStates { get; set; }
    }
}
