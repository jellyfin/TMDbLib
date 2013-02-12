using System.Collections.Generic;

namespace TMDbLib.Objects.Movies
{
    public class Keywords
    {
        public int Id { get; set; }
        public List<Keyword> keywords { get; set; }
    }
}