﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

public class CrewAggregate : CrewBase
{
    [JsonProperty("jobs")]
    public List<CrewJob> Jobs { get; set; }

    [JsonProperty("total_episode_count")]
    public int TotalEpisodeCount { get; set; }
}
