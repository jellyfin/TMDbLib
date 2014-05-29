using System;
using Newtonsoft.Json;
using TMDbLib.Converters;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.TvShows
{
    public class TvEpisode
    {
        /// <summary>
        /// Object Id, will only be populated when explicitly getting episode details
        /// </summary>
        [JsonProperty("id")]
        public int? Id { get; set; }
        /// <summary>
        /// Will only be populated when explicitly getting an episode
        /// </summary>
        [JsonProperty("season_number")]
        public int? SeasonNumber { get; set; }
        [JsonProperty("episode_number")]
        public int EpisodeNumber { get; set; }
        [JsonProperty("air_date")]
        [JsonConverter(typeof(DateTimeConverterYearMonthDay))]
        public DateTime AirDate { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("overview")]
        public string Overview { get; set; }
        [JsonProperty("still_path")]
        public string StillPath { get; set; }
        [JsonProperty("production_code")]
        public string ProductionCode { get; set; } // TODO check type, was null in the apiary
        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }
        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty("credits")]
        public Credits Credits { get; set; }
        [JsonProperty("external_ids")]
        public ExternalIds ExternalIds { get; set; }
        [JsonProperty("images")]
        public Images Images { get; set; }
    }
}
