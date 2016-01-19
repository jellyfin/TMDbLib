using Newtonsoft.Json;

namespace TMDbLib.Objects.Credit
{
    public class Credit
    {
        [JsonProperty("credit_type")]
        public string CreditType { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("media")]
        public CreditMedia Media { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("person")]
        public CreditPerson Person { get; set; }
    }
}