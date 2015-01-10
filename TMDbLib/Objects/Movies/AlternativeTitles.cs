using System.Collections.Generic;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Movies
{
    public class AlternativeTitles
    {
        public int Id { get; set; }
        public List<AlternativeTitle> Titles { get; set; }
    }
}