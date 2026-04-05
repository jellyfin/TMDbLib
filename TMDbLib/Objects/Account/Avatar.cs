using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Account;

/// <summary>
/// Represents an avatar associated with an account.
/// </summary>
public class Avatar
{
    /// <summary>
    /// Gets or sets the Gravatar information.
    /// </summary>
    [JsonPropertyName("gravatar")]
    public Gravatar? Gravatar { get; set; }
}
