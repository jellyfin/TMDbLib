using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General
{
    //[DJ] [Serializable]
    public class TMDbConfig
    {
        [JsonProperty("images")]
        public ConfigImageTypes Images { get; set; }

        [JsonProperty("change_keys")]
        public List<string> ChangeKeys { get; set; }
    }
}