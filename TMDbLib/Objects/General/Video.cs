using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a video (trailer, teaser, clip, etc.) associated with a media item.
/// </summary>
public class Video
{
    /// <summary>
    /// Gets or sets the video ID.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets a country code, e.g. US.
    /// </summary>
    [JsonProperty("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonProperty("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the video key (used to construct the video URL).
    /// </summary>
    [JsonProperty("key")]
    public string? Key { get; set; }

    /// <summary>
    /// Gets or sets the name of the video.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the video is official.
    /// </summary>
    [JsonProperty("official")]
    public bool Official { get; set; }

    /// <summary>
    /// Gets or sets the date the video was published.
    /// </summary>
    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; set; }

    /// <summary>
    /// Gets or sets the site where the video is hosted (e.g., YouTube).
    /// </summary>
    [JsonProperty("site")]
    public string? Site { get; set; }

    /// <summary>
    /// Gets or sets the size/resolution of the video.
    /// </summary>
    [JsonProperty("size")]
    public int Size { get; set; }

    /// <summary>
    /// Gets or sets the type of video (e.g., Trailer, Teaser, Clip).
    /// </summary>
    [JsonProperty("type")]
    public string? Type { get; set; }
}
