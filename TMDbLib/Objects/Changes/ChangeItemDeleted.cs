using Newtonsoft.Json;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Represents a change item for a deleted action.
/// </summary>
public class ChangeItemDeleted : ChangeItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeItemDeleted"/> class.
    /// </summary>
    public ChangeItemDeleted()
    {
        Action = ChangeAction.Deleted;
    }

    /// <summary>
    /// Gets or sets the original value before deletion.
    /// </summary>
    [JsonProperty("original_value")]
    public object? OriginalValue { get; set; }
}
