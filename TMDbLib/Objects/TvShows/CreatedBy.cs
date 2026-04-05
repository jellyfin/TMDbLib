using System.Text.Json.Serialization;
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
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonPropertyName("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the name of the person.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the gender of the person.
    /// </summary>
    [JsonPropertyName("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets the profile image path.
    /// </summary>
    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }
}
