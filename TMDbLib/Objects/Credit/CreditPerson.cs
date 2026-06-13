using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;

namespace TMDbLib.Objects.Credit;

/// <summary>
/// Person in a credit context.
/// </summary>
public class CreditPerson
{
    /// <summary>
    /// Gets or sets a value indicating whether the person is associated with adult content.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the gender.
    /// </summary>
    [JsonProperty("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets the TMDb id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the department the person is known for.
    /// </summary>
    [JsonProperty("known_for_department")]
    public string? KnownForDepartment { get; set; }

    /// <summary>
    /// Gets or sets the media type discriminator.
    /// </summary>
    [JsonProperty("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the original (untranslated) name.
    /// </summary>
    [JsonProperty("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonProperty("popularity")]
    public double Popularity { get; set; }

    /// <summary>
    /// Gets or sets the profile image path.
    /// </summary>
    [JsonProperty("profile_path")]
    public string? ProfilePath { get; set; }
}
