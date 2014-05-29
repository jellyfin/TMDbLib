using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    public class AlternativeTitles
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("titles")]
        public List<AlternativeTitle> Titles { get; set; }
    }
}