using System.Text.Json.Serialization;
using TMDbLib.Objects.People;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents the base information for a crew member.
/// </summary>
[JsonDerivedType(typeof(Crew))]
[JsonDerivedType(typeof(CrewAggregate))]
public class CrewBase
{
    /// <summary>
    /// Gets or sets the department the crew member works in.
    /// </summary>
    [JsonPropertyName("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID of the crew member.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the crew member.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the profile image path for the crew member.
    /// </summary>
    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }

    /// <summary>
    /// Gets or sets the gender of the crew member.
    /// </summary>
    [JsonPropertyName("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the crew member is an adult performer.
    /// </summary>
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the department the crew member is known for.
    /// </summary>
    [JsonPropertyName("known_for_department")]
    public string? KnownForDepartment { get; set; }

    /// <summary>
    /// Gets or sets the original name of the crew member.
    /// </summary>
    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the popularity score of the crew member.
    /// </summary>
    [JsonPropertyName("popularity")]
    public float Popularity { get; set; }
}
