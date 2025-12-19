using Newtonsoft.Json;

namespace TMDbLib.Objects.Account;

/// <summary>
/// Represents a Gravatar image hash.
/// </summary>
public class Gravatar
{
    /// <summary>
    /// Gets or sets the Gravatar hash value.
    /// </summary>
    [JsonProperty("hash")]
    public string Hash { get; set; }
}
