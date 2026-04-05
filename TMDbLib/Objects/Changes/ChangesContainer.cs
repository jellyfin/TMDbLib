using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Contains a collection of changes for an entity.
/// </summary>
public class ChangesContainer
{
    /// <summary>
    /// Gets or sets the list of changes.
    /// </summary>
    [JsonPropertyName("changes")]
    public List<Change>? Changes { get; set; }
}
