using System;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a video (trailer, teaser, clip, etc.) associated with a media item.
/// </summary>
public class Video
{
    /// <summary>
    /// Gets or sets the video ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets a country code, e.g. US.
    /// </summary>
    [JsonPropertyName("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonPropertyName("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the video key (used to construct the video URL).
    /// </summary>
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    /// <summary>
    /// Gets or sets the name of the video.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the video is official.
    /// </summary>
    [JsonPropertyName("official")]
    public bool Official { get; set; }

    /// <summary>
    /// Gets or sets the date the video was published.
    /// </summary>
    [JsonPropertyName("published_at")]
    public DateTime PublishedAt { get; set; }

    /// <summary>
    /// Gets or sets the site where the video is hosted (e.g., YouTube).
    /// </summary>
    [JsonPropertyName("site")]
    public string? Site { get; set; }

    /// <summary>
    /// Gets or sets the size/resolution of the video.
    /// </summary>
    [JsonPropertyName("size")]
    public int Size { get; set; }

    /// <summary>
    /// Gets or sets the type of video (e.g., Trailer, Teaser, Clip).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
