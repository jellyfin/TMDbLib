using System;

namespace TMDbLib.Objects.Search
{
    public class SearchTvEpisode
    {
        public int Id { get; set; }
        public int ShowId { get; set; }
        public int EpisodeNumber { get; set; }
        public int SeasonNumber { get; set; }
        public DateTime? AirDate { get; set; }
        public string StillPath { get; set; }
        public double Rating { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
    }
}