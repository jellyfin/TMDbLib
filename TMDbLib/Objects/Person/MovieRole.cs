using System;

namespace TMDbLib.Objects.Person
{
    public class MovieRole
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Character { get; set; }
        public string OriginalTitle { get; set; }
        public string PosterPath { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool Adult { get; set; }
    }
}