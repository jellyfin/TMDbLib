using System.Collections.Generic;

namespace TMDbLib.Objects.Search
{
    // TODO: Join this with SearchContainer, Lists, MovieResultContainer, ChangesListContainer
    public class SearchContainer<T>
    {
        public int Page { get; set; }
        public List<T> Results { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}