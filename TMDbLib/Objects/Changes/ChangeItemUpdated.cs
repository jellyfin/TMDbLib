using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Represents a change item for an updated action.
/// </summary>
public class ChangeItemUpdated : ChangeItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeItemUpdated"/> class.
    /// </summary>
    public ChangeItemUpdated()
    {
        Action = ChangeAction.Updated;
    }

    /// <summary>
    /// Gets or sets the original value before the update.
    /// </summary>
    [JsonPropertyName("original_value")]
    public object? OriginalValue { get; set; }

    /// <summary>
    /// Gets or sets the new value after the update.
    /// </summary>
    [JsonPropertyName("value")]
    public object? Value { get; set; }
}
