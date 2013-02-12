using System.Collections.Generic;
using TMDbLib.Objects.Movies;

namespace TMDbLib.Objects.People
{
    public class Person
    {
        public bool Adult { get; set; }
        public List<string> AlsoKnownAs { get; set; }       // Todo: Find actor with an alternate name to test this
        public string Biography { get; set; }
        public string Birthday { get; set; }        // In this context, dates come as empty strings when unavailable. This can't be parsed.
        public string Deathday { get; set; }        // In this context, dates come as empty strings when unavailable. This can't be parsed.
        public string Homepage { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlaceOfBirth { get; set; }
        public string ProfilePath { get; set; }
        public Credits Credits { get; set; }
        public ProfileImages Images { get; set; }
        public ChangesContainer Changes { get; set; }
    }
}