using System.Collections.Generic;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Movies
{
    public class KeywordsContainer
    {
        public int Id { get; set; }
        public List<Keyword> Keywords { get; set; }
    }
}