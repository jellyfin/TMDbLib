using System;
using System.Collections.Generic;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;

namespace TMDbLib.Objects.TvShows
{
    public class TvEpisode
    {
        /// <summary>
        /// Object Id, will only be populated when explicitly getting episode details
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Will only be populated when explicitly getting an episode
        /// </summary>
        public int? SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public DateTime AirDate { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public string StillPath { get; set; }
        public string ProductionCode { get; set; } // TODO check type, was null in the apiary
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }

        public List<Crew> Crew { get; set; }
        public List<Cast> GuestStars { get; set; }

        public Credits Credits { get; set; }
        public ExternalIds ExternalIds { get; set; }
        public StillImages Images { get; set; }
        public ResultContainer<Video> Videos { get; set; }
        public AccountState AccountStates { get; set; }
    }
}
