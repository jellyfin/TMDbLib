using Newtonsoft.Json;
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
    [JsonProperty("cast_id")]
    public int CastId { get; set; }

    /// <summary>
    /// Gets or sets the character name.
    /// </summary>
    [JsonProperty("character")]
    public string? Character { get; set; }

    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonProperty("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the person ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the person name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the order in the cast.
    /// </summary>
    [JsonProperty("order")]
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the profile image path.
    /// </summary>
    [JsonProperty("profile_path")]
    public string? ProfilePath { get; set; }

    /// <summary>
    /// Gets or sets the gender.
    /// </summary>
    [JsonProperty("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the person is an adult performer.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the department the person is known for.
    /// </summary>
    [JsonProperty("known_for_department")]
    public string? KnownForDepartment { get; set; }

    /// <summary>
    /// Gets or sets the original name.
    /// </summary>
    [JsonProperty("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonProperty("popularity")]
    public float Popularity { get; set; }
}
