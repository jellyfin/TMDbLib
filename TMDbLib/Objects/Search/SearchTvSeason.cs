using System;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Search;

public class SearchTvSeason : SearchBase
{
    public SearchTvSeason()
    {
        MediaType = MediaType.Season;
    }

    [JsonProperty("air_date")]
    public DateTime? AirDate { get; set; }

    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("overview")]
    public string Overview { get; set; }

    [JsonProperty("poster_path")]
    public string PosterPath { get; set; }

    [JsonProperty("season_number")]
    public int SeasonNumber { get; set; }
}
