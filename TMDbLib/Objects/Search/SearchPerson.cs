using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a person search result.
/// </summary>
public class SearchPerson : SearchBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPerson"/> class.
    /// </summary>
    public SearchPerson()
    {
        MediaType = MediaType.Person;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the person is associated with adult content.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the list of movies and TV shows the person is known for.
    /// </summary>
    [JsonProperty("known_for", ItemConverterType = typeof(KnownForConverter))]
    public List<KnownForBase>? KnownFor { get; set; }

    /// <summary>
    /// Gets or sets the name of the person.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the profile image path.
    /// </summary>
    [JsonProperty("profile_path")]
    public string? ProfilePath { get; set; }
}
