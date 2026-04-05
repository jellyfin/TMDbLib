using System.Text.Json.Serialization;
using TMDbLib.Objects.People;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Base class for TV show cast members.
/// </summary>
[JsonDerivedType(typeof(Cast))]
[JsonDerivedType(typeof(CastAggregate))]
public class CastBase
{
    /// <summary>
    /// Gets or sets the person ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the person.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the order of the cast member in the credits.
    /// </summary>
    [JsonPropertyName("order")]
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the profile image path.
    /// </summary>
    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }

    /// <summary>
    /// Gets or sets the gender of the person.
    /// </summary>
    [JsonPropertyName("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the person is associated with adult content.
    /// </summary>
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the department the person is known for.
    /// </summary>
    [JsonPropertyName("known_for_department")]
    public string? KnownForDepartment { get; set; }

    /// <summary>
    /// Gets or sets the original name of the person.
    /// </summary>
    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the popularity score of the person.
    /// </summary>
    [JsonPropertyName("popularity")]
    public float Popularity { get; set; }
}
