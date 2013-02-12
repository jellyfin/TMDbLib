using System.Collections.Generic;

namespace TMDbLib.Objects.Movies
{
    public class MovieListContainer
    {
        public int Page { get; set; }
        public List<MovieResult> Results { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}