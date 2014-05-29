using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Objects.Find
{
    public class FindContainer
    {
        [JsonProperty("movie_results")]
        public List<MovieResult> MovieResults { get; set; }
        [JsonProperty("person_results")]
        public List<Person> PersonResults { get; set; }     // Unconfirmed type
        [JsonProperty("tv_results")]
        public List<TvShowBase> TvResults { get; set; }
    }
}