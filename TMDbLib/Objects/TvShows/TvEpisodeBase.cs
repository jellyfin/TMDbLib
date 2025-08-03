﻿using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

public class TvEpisodeBase : TvEpisodeInfo
{
    [JsonProperty("air_date")]
    public DateTime? AirDate { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("overview")]
    public string Overview { get; set; }

    [JsonProperty("production_code")]
    public string ProductionCode { get; set; }

    [JsonProperty("show_id")]
    public int ShowId { get; set; }

    [JsonProperty("still_path")]
    public string StillPath { get; set; }

    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }

    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }

    [JsonProperty("runtime")]
    public int? Runtime { get; set; }

    [JsonProperty("episode_type")]
    public string EpisodeType { get; set; }
}
