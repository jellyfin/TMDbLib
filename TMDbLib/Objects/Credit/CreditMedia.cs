using System.Collections.Generic;

namespace TMDbLib.Objects.Credit
{
    public class CreditMedia
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Character { get; set; }
        public List<CreditEpisode> Episodes { get; set; }
        public List<CreditSeason> Seasons { get; set; }
    }
}