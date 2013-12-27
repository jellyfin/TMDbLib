using System.Collections.Generic;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Objects.Find
{
    public class FindContainer
    {
        public List<MovieResult> MovieResults { get; set; }
        public List<Person> PersonResults { get; set; }     // Unconfirmed type
        public List<TvShowBase> TvResults { get; set; }
    }
}