using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.People
{
    public class TaggedImage
    {
        [JsonProperty("aspect_ratio")]
        public double AspectRatio { get; set; }

        [JsonProperty("file_path")]
        public string FilePath { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("iso_639_1")]
        public string Iso_639_1 { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("image_type")]
        public string ImageType { get; set; }       // TODO: Turn into enum

        [JsonProperty("media")]
        public Media Media { get; set; }

        [JsonProperty("media_type")]
        public MediaType MediaType { get; set; }
    }
}