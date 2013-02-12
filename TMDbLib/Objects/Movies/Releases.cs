using System.Collections.Generic;

namespace TMDbLib.Objects.Movies
{
    public class Releases
    {
        public int Id { get; set; }
        public List<Country> Countries { get; set; }
    }
}