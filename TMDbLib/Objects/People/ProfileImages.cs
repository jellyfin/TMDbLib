using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.People;

/// <summary>
/// Profile images for a person.
/// </summary>
public class ProfileImages
{
    /// <summary>
    /// Gets or sets the person id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the profile images.
    /// </summary>
    [JsonProperty("profiles")]
    public List<ImageData>? Profiles { get; set; }
}
