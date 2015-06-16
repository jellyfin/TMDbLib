using System;
using System.Collections.Generic;
using TMDbLib.Objects.Movies;

namespace TMDbLib.Objects.People
{
    public class Person
    {
        public bool Adult { get; set; }
        public List<string> AlsoKnownAs { get; set; }
        public string Biography { get; set; }
		public DateTime? Birthday { get; set; }
		public DateTime? Deathday { get; set; }
        public string Homepage { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlaceOfBirth { get; set; }
        public string ProfilePath { get; set; }
        public Credits Credits { get; set; }
        public ProfileImages Images { get; set; }
        public ChangesContainer Changes { get; set; }
        public string ImdbId { get; set; }
        public double Popularity { get; set; }
    }
}