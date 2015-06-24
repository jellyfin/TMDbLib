using System.Collections.Generic;

namespace TMDbLib.Objects.Search
{
    public class SearchPerson
    {
        public int Id { get; set; }
        public bool Adult { get; set; }
        public string Name { get; set; }
        public string ProfilePath { get; set; }
        public double Popularity { get; set; }

        public List<SearchMovie> KnownFor { get; set; }
    }
}