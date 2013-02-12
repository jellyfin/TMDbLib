using System.Collections.Generic;

namespace TMDbLib.Objects.Movies
{
    public class Trailers
    {
        public int Id { get; set; }
        public List<object> Quicktime { get; set; } // TODO: Fix object type
        public List<Youtube> Youtube { get; set; }
    }
}