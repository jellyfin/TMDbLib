using System.Collections.Generic;

namespace TMDbLib.Objects.Movies
{
    public class Casts
    {
        public int Id { get; set; }
        public List<Cast> Cast { get; set; }
        public List<Crew> Crew { get; set; }
    }
}