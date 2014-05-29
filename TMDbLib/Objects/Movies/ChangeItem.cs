using System;
using Newtonsoft.Json;
using TMDbLib.Converters;

namespace TMDbLib.Objects.Movies
{
    public class ChangeItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverterYearMonthDayHourMinuteSecondUtc))]
        public DateTime Time { get; set; }
        [JsonProperty("iso_639_1")]
        public string Iso_639_1 { get; set; }
        [JsonProperty("value")]
        public object Value { get; set; }
    }
}