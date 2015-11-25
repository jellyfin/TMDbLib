using Newtonsoft.Json;

namespace TMDbLib.Objects.Credit
{
    public class CreditPerson
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}