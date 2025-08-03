using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows
{
    public class CastAggregate : CastBase
    {
        [JsonProperty("roles")]
        public List<CastRole> Roles { get; set; }

        [JsonProperty("total_episode_count")]
        public int TotalEpisodeCount { get; set; }
    }
}
