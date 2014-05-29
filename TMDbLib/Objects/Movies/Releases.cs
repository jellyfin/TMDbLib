using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    public class Releases
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("countries")]
        public List<Country> Countries { get; set; }
    }
}