using System.Collections.Generic;

namespace TMDbLib.Objects.People
{
    public class TvCredits
    {
        public int Id { get; set; }
        public List<TvRole> Cast { get; set; }
        public List<TvJob> Crew { get; set; }
    }
}