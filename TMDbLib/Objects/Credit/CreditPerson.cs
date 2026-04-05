using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Credit;

/// <summary>
/// Represents a person in a credit context.
/// </summary>
public class CreditPerson
{
    /// <summary>
    /// Gets or sets the person's unique identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the person's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
