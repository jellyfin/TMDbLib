using System.Collections.Generic;

namespace TMDbLib.Objects.Changes
{
    // TODO: Join this with SearchContainer, Lists, MovieResultContainer, ChangesListContainer
    public class ChangesListContainer
    {
        public List<ChangesListItem> Results { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}