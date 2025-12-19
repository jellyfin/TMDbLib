namespace TMDbLib.Objects.Changes;

/// <summary>
/// Represents a change item for a created action.
/// </summary>
public class ChangeItemCreated : ChangeItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeItemCreated"/> class.
    /// </summary>
    public ChangeItemCreated()
    {
        Action = ChangeAction.Created;
    }
}
