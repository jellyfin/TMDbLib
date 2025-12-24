using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Contains a collection of changes for an entity.
/// </summary>
public class ChangesContainer
{
    /// <summary>
    /// Gets or sets the list of changes.
    /// </summary>
    [JsonProperty("changes")]
    public List<Change>? Changes { get; set; }
}
