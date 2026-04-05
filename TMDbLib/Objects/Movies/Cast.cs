using System.Text.Json.Serialization;
using TMDbLib.Objects.People;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Represents a cast member in a movie.
/// </summary>
public class Cast
{
    /// <summary>
    /// Gets or sets the cast ID.
    /// </summary>
    [JsonPropertyName("cast_id")]
    public int CastId { get; set; }

    /// <summary>
    /// Gets or sets the character name.
    /// </summary>
    [JsonPropertyName("character")]
    public string? Character { get; set; }

    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonPropertyName("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the person ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the person name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the order in the cast.
    /// </summary>
    [JsonPropertyName("order")]
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the profile image path.
    /// </summary>
    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }

    /// <summary>
    /// Gets or sets the gender.
    /// </summary>
    [JsonPropertyName("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the person is an adult performer.
    /// </summary>
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the department the person is known for.
    /// </summary>
    [JsonPropertyName("known_for_department")]
    public string? KnownForDepartment { get; set; }

    /// <summary>
    /// Gets or sets the original name.
    /// </summary>
    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonPropertyName("popularity")]
    public float Popularity { get; set; }
}
