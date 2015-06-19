using System;

namespace TMDbLib.Objects.People
{
    public class TvRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Character { get; set; }
        public string OriginalName { get; set; }
        public string PosterPath { get; set; }
        public DateTime? FirstAirDate { get; set; }
        public int EpisodeCount { get; set; }
        public string CreditId { get; set; }
    }
}