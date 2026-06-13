using Newtonsoft.Json;
using TMDbLib.Objects.People;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Creator of a TV show.
/// </summary>
public class CreatedBy
{
    /// <summary>
    /// Gets or sets the person id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the credit id.
    /// </summary>
    [JsonProperty("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the gender.
    /// </summary>
    [JsonProperty("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets the profile image path.
    /// </summary>
    [JsonProperty("profile_path")]
    public string? ProfilePath { get; set; }
}
