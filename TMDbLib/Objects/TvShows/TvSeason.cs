using System;
using System.Collections.Generic;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.TvShows
{
    public class TvSeason
    {
        /// <summary>
        /// Object Id, will only be populated when explicitly getting episode details
        /// </summary>
        public int? Id { get; set; }
        public int SeasonNumber { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public DateTime? AirDate { get; set; }
        public string PosterPath { get; set; }
        public int EpisodeCount { get; set; }

        public List<TvEpisode> Episodes { get; set; }
        public Images Images { get; set; }
        public Credits Credits { get; set; }
        public ExternalIds ExternalIds { get; set; }
        public ResultContainer<Video> Videos { get; set; }
    }
}
