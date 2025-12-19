using Newtonsoft.Json;

namespace TMDbLib.Objects.Account;

/// <summary>
/// Represents an avatar associated with an account.
/// </summary>
public class Avatar
{
    /// <summary>
    /// Gets or sets the Gravatar information.
    /// </summary>
    [JsonProperty("gravatar")]
    public Gravatar Gravatar { get; set; }
}
