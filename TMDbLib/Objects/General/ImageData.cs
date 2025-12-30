using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents image data including metadata and voting information.
/// </summary>
public class ImageData
{
    /// <summary>
    /// Gets or sets the aspect ratio of the image.
    /// </summary>
    [JsonProperty("aspect_ratio")]
    public double AspectRatio { get; set; }

    /// <summary>
    /// Gets or sets the file path for the image.
    /// </summary>
    [JsonProperty("file_path")]
    public string? FilePath { get; set; }

    /// <summary>
    /// Gets or sets the height of the image in pixels.
    /// </summary>
    [JsonProperty("height")]
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the language code, e.g. en.
    /// </summary>
    [JsonProperty("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the country code for the image.
    /// </summary>
    [JsonProperty("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets the average vote score for the image.
    /// </summary>
    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the number of votes for the image.
    /// </summary>
    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }

    /// <summary>
    /// Gets or sets the width of the image in pixels.
    /// </summary>
    [JsonProperty("width")]
    public int Width { get; set; }
}
