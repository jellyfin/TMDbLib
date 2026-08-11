using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Represents a change item for an added action.
/// </summary>
public class ChangeItemAdded : ChangeItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeItemAdded"/> class.
    /// </summary>
    public ChangeItemAdded()
    {
        Action = ChangeAction.Added;
    }

    /// <summary>
    /// Gets or sets the value that was added.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonConverter(typeof(TmdbChangeValueConverter))]
    public object? Value { get; set; }
}
