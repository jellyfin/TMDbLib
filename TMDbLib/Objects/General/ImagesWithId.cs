using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a collection of images that includes an ID property.
/// </summary>
public class ImagesWithId : Images
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
