using System.Collections.Generic;

namespace TMDbLib.Objects.General
{
    public class MovieResultContainer
    {
        public int Id { get; set; }
        public int Page { get; set; }
        public List<MovieResult> Results { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}