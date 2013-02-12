using System.Collections.Generic;

namespace TMDbLib.Objects.Movies
{
    public class TranslationsContainer
    {
        public int Id { get; set; }
        public List<Translation> Translations { get; set; }
    }
}