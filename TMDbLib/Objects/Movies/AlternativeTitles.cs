using System.Collections.Generic;

namespace TMDbLib.Objects.Movies
{
    public class AlternativeTitles
    {
        public int Id { get; set; }
        public List<AlternativeTitle> Titles { get; set; }
    }
}