using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.TvShows
{
    public class TvSeason
    {
        [JsonProperty("account_states")]
        public ResultContainer<TvEpisodeAccountStateWithNumber> AccountStates { get; set; }

        [JsonProperty("air_date")]
        public DateTime? AirDate { get; set; }

        [JsonProperty("credits")]
        public Credits Credits { get; set; }

        // Special field that will only be populated when you do a tvshow retrieval withouth season details, patched for consistency sake
        private int _episodeCount;
        [JsonProperty("episode_count")]
        public int EpisodeCount
        {
            get { return Episodes == null ? _episodeCount : Episodes.Count; }
            set { _episodeCount = value; }
        }

        [JsonProperty("episodes")]
        public List<TvSeasonEpisode> Episodes { get; set; }

        [JsonProperty("external_ids")]
        public ExternalIdsTvSeason ExternalIds { get; set; }

        /// <summary>
        /// Object Id, will only be populated when explicitly getting episode details
        /// </summary>
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("images")]
        public PosterImages Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("season_number")]
        public int SeasonNumber { get; set; }

        [JsonProperty("videos")]
        public ResultContainer<Video> Videos { get; set; }
    }
}
