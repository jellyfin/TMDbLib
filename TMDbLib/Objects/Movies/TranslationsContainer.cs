using System.Collections.Generic;

namespace TMDbLib.Objects.Movies
{
    // TODO: Can be replaced with ResultContainer<Translation>
    // TODO: Move into generic objects
    public class TranslationsContainer
    {
        public int Id { get; set; }
        public List<Translation> Translations { get; set; }
    }
}