using System.Collections.Generic;

namespace TMDbLib.Objects.Movies
{
    public class KeywordsContainer
    {
        public int Id { get; set; }
        public List<Keyword> Keywords { get; set; }
    }
}