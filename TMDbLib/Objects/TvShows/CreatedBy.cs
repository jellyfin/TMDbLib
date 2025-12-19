using Newtonsoft.Json;
using TMDbLib.Objects.People;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents a person who created the TV show.
/// </summary>
public class CreatedBy
{
    /// <summary>
    /// Gets or sets the person ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonProperty("credit_id")]
    public string CreditId { get; set; }

    /// <summary>
    /// Gets or sets the name of the person.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the gender of the person.
    /// </summary>
    [JsonProperty("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets the profile image path.
    /// </summary>
    [JsonProperty("profile_path")]
    public string ProfilePath { get; set; }
}
