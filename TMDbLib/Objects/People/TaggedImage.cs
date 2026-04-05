using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents an image tagged with a person.
/// </summary>
public class TaggedImage
{
    /// <summary>
    /// Gets or sets the aspect ratio.
    /// </summary>
    [JsonPropertyName("aspect_ratio")]
    public double AspectRatio { get; set; }

    /// <summary>
    /// Gets or sets the file path.
    /// </summary>
    [JsonPropertyName("file_path")]
    public string? FilePath { get; set; }

    /// <summary>
    /// Gets or sets the height in pixels.
    /// </summary>
    [JsonPropertyName("height")]
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the image ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the image type.
    /// </summary>
    [JsonPropertyName("image_type")]
    public string? ImageType { get; set; } // TODO: Turn into enum

    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonPropertyName("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the media item this image is from.
    /// </summary>
    [JsonPropertyName("media")]
    public SearchBase? Media { get; set; }

    /// <summary>
    /// Gets or sets the media type.
    /// </summary>
    [JsonPropertyName("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the vote count.
    /// </summary>
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    /// <summary>
    /// Gets or sets the width in pixels.
    /// </summary>
    [JsonPropertyName("width")]
    public int Width { get; set; }
}
