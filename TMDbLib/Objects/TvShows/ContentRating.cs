using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows
{
    /// <summary>
    /// Content rating for a TV show in a country.
    /// </summary>
    public class ContentRating
    {
        /// <summary>
        /// Gets or sets the rating descriptors.
        /// </summary>
        [JsonProperty("descriptors")]
        public List<string>? Descriptors { get; set; }

        /// <summary>
        /// Gets or sets the country code, e.g. US.
        /// </summary>
        [JsonProperty("iso_3166_1")]
        public string? Iso_3166_1 { get; set; }

        /// <summary>
        /// Gets or sets the content rating value.
        /// </summary>
        [JsonProperty("rating")]
        public string? Rating { get; set; }
    }
}
