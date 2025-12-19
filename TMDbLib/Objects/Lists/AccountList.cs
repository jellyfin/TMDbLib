using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Lists;

/// <summary>
/// Represents an account-specific list.
/// </summary>
public class AccountList : TMDbList<int>
{
    /// <summary>
    /// Gets or sets the media type of the list.
    /// </summary>
    [JsonProperty("list_type")]
    public MediaType ListType { get; set; }
}
