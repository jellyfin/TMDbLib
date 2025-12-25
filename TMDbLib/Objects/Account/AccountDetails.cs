using Newtonsoft.Json;

namespace TMDbLib.Objects.Account;

/// <summary>
/// Represents detailed information about a TMDb user account.
/// </summary>
public class AccountDetails
{
    /// <summary>
    /// Gets or sets the avatar information.
    /// </summary>
    [JsonProperty("avatar")]
    public Avatar? Avatar { get; set; }

    /// <summary>
    /// Gets or sets the account ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether adult content is included.
    /// </summary>
    [JsonProperty("include_adult")]
    public bool IncludeAdult { get; set; }

    /// <summary>
    /// Gets or sets a country code, e.g. US.
    /// </summary>
    [JsonProperty("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonProperty("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the account name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [JsonProperty("username")]
    public string? Username { get; set; }
}
