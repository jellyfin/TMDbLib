using System;

namespace TMDbLib.Objects.Person
{
    public class MovieJob
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Department { get; set; }
        public string Job { get; set; }
        public string PosterPath { get; set; }
		public DateTime? ReleaseDate { get; set; }
        public bool Adult { get; set; }
    }
}