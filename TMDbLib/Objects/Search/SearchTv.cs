using System;
using System.Collections.Generic;

namespace TMDbLib.Objects.Search
{
    public class SearchTv
    {
        public int Id { get; set; }
        public string BackdropPath { get; set; }
        public string OriginalName { get; set; }
        public DateTime? FirstAirDate { get; set; }
        public string PosterPath { get; set; }
        public double Popularity { get; set; }
        public string Name { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public List<string> OrigionCountry { get; set; }
    }
}