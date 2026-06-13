using Newtonsoft.Json;
using TMDbLib.Objects.People;

namespace TMDbLib.Objects.General.Schema;

/// <summary>
/// Shared shape for the person-summary object TMDb embeds inside cast/crew lists,
/// search results, and person/credit responses.
/// </summary>
public class TmdbPersonSummary : TmdbEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TmdbPersonSummary"/> class
    /// with <see cref="TmdbEntity.MediaType"/> set to <see cref="MediaType.Person"/>.
    /// </summary>
    public TmdbPersonSummary()
    {
        MediaType = MediaType.Person;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the person is associated with adult content.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the person's gender.
    /// </summary>
    [JsonProperty("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets the department TMDb classifies the person under.
    /// </summary>
    [JsonProperty("known_for_department")]
    public string? KnownForDepartment { get; set; }

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the original (untranslated) name.
    /// </summary>
    [JsonProperty("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the profile image path.
    /// </summary>
    [JsonProperty("profile_path")]
    public string? ProfilePath { get; set; }
}
