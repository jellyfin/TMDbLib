using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents a collection of profile images for a person.
/// </summary>
public class ProfileImages
{
    /// <summary>
    /// Gets or sets the person ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of profile images.
    /// </summary>
    [JsonProperty("profiles")]
    public List<ImageData> Profiles { get; set; }
}
