using System;

namespace TMDbLib.Objects.Movies
{
    public class MovieResult
    {
        public string BackdropPath { get; set; }
        public int Id { get; set; }
        public string OriginalTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterPath { get; set; }
        public string Title { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
    }
}