using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General
{
    public class Job
    {
        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("job_list")]
        public List<string> JobList { get; set; }
    }
}
