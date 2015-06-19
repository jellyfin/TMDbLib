using System.Collections.Generic;

namespace TMDbLib.Objects.People
{
    public class MovieCredits
    {
        public int Id { get; set; }
        public List<MovieRole> Cast { get; set; }
        public List<MovieJob> Crew { get; set; }
    }
}