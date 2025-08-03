using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Reviews;

public class ReviewBase
{
    [JsonProperty("author")]
    public string Author { get; set; }

    [JsonProperty("author_details")]
    public AuthorDetails AuthorDetails { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
