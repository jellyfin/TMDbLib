using System;

namespace TMDbLib.Objects.Movies
{
    public class Country
    {
        public string Iso_3166_1 { get; set; }
        public string Certification { get; set; }
        public bool Primary { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}
