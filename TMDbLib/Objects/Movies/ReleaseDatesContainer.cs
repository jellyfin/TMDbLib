using System.Collections.Generic;

namespace TMDbLib.Objects.Movies
{
    public class ReleaseDatesContainer
    {
        // TODO: This could be made into an inherited class with Iso3166 as a base-clas property
        public string Iso_3166_1 { get; set; }

        public List<ReleaseDateItem> ReleaseDates { get; set; }
    }
}