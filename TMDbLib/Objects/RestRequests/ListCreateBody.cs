using System.Text.Json.Serialization;

namespace TMDbLib.Objects.RestRequests;

/// <summary>
/// Represents a request body for creating a list.
/// </summary>
public sealed class ListCreateBody : IBody
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ListCreateBody"/> class.
    /// </summary>
    /// <param name="name">The list name.</param>
    /// <param name="description">The list description.</param>
    /// <param name="language">The language code for the list.</param>
    public ListCreateBody(string name, string description, string? language = null)
    {
        Name = name;
        Description = description;
        Language = language;
    }

    /// <summary>
    /// Gets or sets the list name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the list description.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the language code for the list.
    /// </summary>
    [JsonPropertyName("language")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Language { get; set; }
}
