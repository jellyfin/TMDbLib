using Newtonsoft.Json;

namespace TMDbLib.Objects.Account
{
    public class AccountDetails
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("avatar")]
        public Avatar Avatar { get; set; }

        [JsonProperty("include_adult")]
        public bool IncludeAdult { get; set; }

        /// <summary>
        /// The country iso code specified by the user. Ex. US
        /// </summary>
        [JsonProperty("iso_3166_1")]
        public string Iso_3166_1 { get; set; }

        /// <summary>
        /// The Language iso code specified by the user. Ex en
        /// </summary>
        [JsonProperty("iso_639_1")]
        public string Iso_639_1 { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
