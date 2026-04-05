using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Account;

/// <summary>
/// Represents detailed information about a TMDb user account.
/// </summary>
public class AccountDetails
{
    /// <summary>
    /// Gets or sets the avatar information.
    /// </summary>
    [JsonPropertyName("avatar")]
    public Avatar? Avatar { get; set; }

    /// <summary>
    /// Gets or sets the account ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether adult content is included.
    /// </summary>
    [JsonPropertyName("include_adult")]
    public bool IncludeAdult { get; set; }

    /// <summary>
    /// Gets or sets a country code, e.g. US.
    /// </summary>
    [JsonPropertyName("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonPropertyName("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the account name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [JsonPropertyName("username")]
    public string? Username { get; set; }
}
