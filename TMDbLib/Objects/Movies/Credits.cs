using System.Collections.Generic;

namespace TMDbLib.Objects.Movies
{
    public class Credits
    {
        public int Id { get; set; }
        public List<Cast> Cast { get; set; }
        public List<Crew> Crew { get; set; }
    }
}