using System;

namespace TMDbLib.Objects.Credit
{
    public class CreditEpisode
    {
        public DateTime? AirDate { get; set; }
        public int EpisodeNumber { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public int SeasonNumber { get; set; }
        public string StillPath { get; set; }
    }
}
