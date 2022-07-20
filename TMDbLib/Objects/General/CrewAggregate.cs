using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.People;

namespace TMDbLib.Objects.General
{
    public class CrewAggregate
    {
        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("jobs")]
        public List<CrewJob> Jobs { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }

        [JsonProperty("gender")]
        public PersonGender Gender { get; set; }

        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("known_for_department")]
        public string KnownForDepartment { get; set; }

        [JsonProperty("original_name")]
        public string OriginalName { get; set; }

        [JsonProperty("popularity")]
        public float Popularity { get; set; }
        
        [JsonProperty("total_episode_count")]
        public int TotalEpisodeCount { get; set; }
    }
}