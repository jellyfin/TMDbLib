using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Lists
{
    public class List
    {
        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("favorite_count")]
        public int FavoriteCount { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("items")]
        public List<MovieResult> Items { get; set; }
        [JsonProperty("item_count")]
        public int ItemCount { get; set; }
        /// <summary>
        /// The Language iso code of a language the list is targeting. Ex en
        /// </summary>
        [JsonProperty("iso_639_1")]
        public string Iso_639_1 { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
        [JsonProperty("listtype")]
        public ContentType ListType { get; set; }
    }
}