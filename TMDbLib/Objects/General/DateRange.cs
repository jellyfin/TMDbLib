using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General
{
    public class DateRange
    {
        [JsonProperty("minimum")]
        public DateTime Minimum { get; set; }

        [JsonProperty("maximum")]
        public DateTime Maximum { get; set; }
    }
}