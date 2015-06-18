using System;
using System.Collections.Generic;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.TvShows
{
    public class TvShow : SearchTv
    {
        public string Overview { get; set; }
        public List<int> EpisodeRunTime { get; set; }
        public string Homepage { get; set; }

        public DateTime? LastAirDate { get; set; }

        public int NumberOfSeasons { get; set; }
        public int NumberOfEpisodes { get; set; }
        public List<TvSeason> Seasons { get; set; }

        public bool InProduction { get; set; }
        public List<ProductionCompany> ProductionCompanies { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }

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
        public string OriginalLanguage { get; set; }

        public Images Images { get; set; }
        public Credits Credits { get; set; }
        public ExternalIds ExternalIds { get; set; }
        public ResultContainer<Video> Videos { get; set; }
        public ResultContainer<ContentRating> ContentRatings { get; set; }
        public ResultContainer<AlternativeTitle> AlternativeTitles { get; set; }
        public ResultContainer<Keyword> Keywords { get; set; }
        public ResultContainer<TvShow> Similar { get; set; }
        public ChangesContainer Changes { get; set; }
        public TranslationsContainer Translations { get; set; }
    }
}
