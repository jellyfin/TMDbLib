using Newtonsoft.Json;
using TMDbLib.Objects.People;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents the base information for a crew member.
/// </summary>
public class CrewBase
{
    /// <summary>
    /// Gets or sets the department the crew member works in.
    /// </summary>
    [JsonProperty("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID of the crew member.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the crew member.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the profile image path for the crew member.
    /// </summary>
    [JsonProperty("profile_path")]
    public string? ProfilePath { get; set; }

    /// <summary>
    /// Gets or sets the gender of the crew member.
    /// </summary>
    [JsonProperty("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the crew member is an adult performer.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the department the crew member is known for.
    /// </summary>
    [JsonProperty("known_for_department")]
    public string? KnownForDepartment { get; set; }

    /// <summary>
    /// Gets or sets the original name of the crew member.
    /// </summary>
    [JsonProperty("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the popularity score of the crew member.
    /// </summary>
    [JsonProperty("popularity")]
    public float Popularity { get; set; }
}
