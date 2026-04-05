using System.Text.Json.Serialization;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.Find;

/// <summary>
/// Represents a person found through external ID search.
/// </summary>
public class FindPerson : SearchPerson
{
    /// <summary>
    /// Gets or sets the gender of the person.
    /// </summary>
    [JsonPropertyName("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets the department the person is known for.
    /// </summary>
    [JsonPropertyName("known_for_department")]
    public string? KnownForDepartment { get; set; }
}
