using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents an image tagged with a person.
/// </summary>
public class TaggedImage
{
    /// <summary>
    /// Gets or sets the aspect ratio.
    /// </summary>
    [JsonProperty("aspect_ratio")]
    public double AspectRatio { get; set; }

    /// <summary>
    /// Gets or sets the file path.
    /// </summary>
    [JsonProperty("file_path")]
    public string? FilePath { get; set; }

    /// <summary>
    /// Gets or sets the height in pixels.
    /// </summary>
    [JsonProperty("height")]
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the image ID.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the image type.
    /// </summary>
    [JsonProperty("image_type")]
    public string? ImageType { get; set; } // TODO: Turn into enum

    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonProperty("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the media item this image is from.
    /// </summary>
    [JsonProperty("media")]
    public SearchBase? Media { get; set; }

    /// <summary>
    /// Gets or sets the media type.
    /// </summary>
    [JsonProperty("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the vote count.
    /// </summary>
    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }

    /// <summary>
    /// Gets or sets the width in pixels.
    /// </summary>
    [JsonProperty("width")]
    public int Width { get; set; }
}
