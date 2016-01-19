using System;

namespace TMDbLib.Objects.Movies
{
    public class ReleaseDateItem
    {
        public string Certification { get; set; }
        public string Iso_639_1 { get; set; }
        public string Note { get; set; }

        public DateTime ReleaseDate { get; set; }

        public ReleaseDateType Type { get; set; }
    }
}