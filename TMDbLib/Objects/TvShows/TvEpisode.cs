using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.TvShows
{
    public class TvEpisode
    {
        [JsonProperty("account_states")]
        public TvAccountState AccountStates { get; set; }

        [JsonProperty("air_date")]
        public DateTime? AirDate { get; set; }

        [JsonProperty("credits")]
        public CreditsWithGuestStars Credits { get; set; }

        [JsonProperty("crew")]
        public List<Crew> Crew { get; set; }

        [JsonProperty("episode_number")]
        public int EpisodeNumber { get; set; }

        [JsonProperty("external_ids")]
        public ExternalIdsTvEpisode ExternalIds { get; set; }

        [JsonProperty("guest_stars")]
        public List<Cast> GuestStars { get; set; }

        /// <summary>
        /// Object Id, will only be populated when explicitly getting episode details
        /// </summary>
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("images")]
        public StillImages Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("production_code")]
        public string ProductionCode { get; set; } // TODO check type, was null in the apiary

        /// <summary>
        /// Will only be populated when explicitly getting an episode
        /// </summary>
        [JsonProperty("season_number")]
        public int? SeasonNumber { get; set; }
        
        [JsonProperty("still_path")]
        public string StillPath { get; set; }

        [JsonProperty("videos")]
        public ResultContainer<Video> Videos { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }
    }
}
