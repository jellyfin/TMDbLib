using System.Collections.Generic;

namespace TMDbLib.Objects.TvShows
{
    public class TvShowWithSeasonDetails : TvShow
    {
        public List<TvSeason> SeasonsWithDetails { get; set; }
    }
}
