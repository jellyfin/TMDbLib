using System;

namespace TMDbLib.Objects.Collections
{
    public class Part
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterPath { get; set; }
        public string BackdropPath { get; set; }
    }
}