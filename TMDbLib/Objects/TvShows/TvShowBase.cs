using System;

namespace TMDbLib.Objects.TvShows
{
    public class TvShowBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }

        public DateTime? FirstAirDate { get; set; }

        public string BackdropPath { get; set; }
        public string PosterPath { get; set; }

        public double Popularity { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
    }
}
