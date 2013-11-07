using System;
using System.Collections.Generic;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;

namespace TMDbLib.Objects.TvShows
{
    public class TvShow : TvShowBase
    {
        public string Overview { get; set; }
        public List<int> EpisodeRunTime { get; set; }
        public string Homepage { get; set; }

        public DateTime? LastAirDate { get; set; }

        public int NumberOfSeasons { get; set; }
        public int NumberOfEpisodes { get; set; }
        public List<TvSeason> Seasons { get; set; }

        public bool InProduction { get; set; }
        public TvShowStatus? Status { get; set; }

        public List<Person> CreatedBy { get; set; }
        public List<Genre> Genres { get; set; }
        /// <summary>
        /// language ISO code ex. en
        /// </summary>
        public List<string> Languages { get; set; }
        public List<Network> Networks { get; set; }
        /// <summary>
        /// Country ISO code ex. US
        /// </summary>
        public List<string> OriginCountry { get; set; }

        public Images Images { get; set; }
        public Credits Credits { get; set; }
        public ExternalIds ExternalIds { get; set; }
    }
}
