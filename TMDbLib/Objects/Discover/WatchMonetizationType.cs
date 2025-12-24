using TMDbLib.Utilities;

namespace TMDbLib.Objects.Discover;

/// <summary>
/// Specifies the monetization type for watch provider filtering.
/// </summary>
public enum WatchMonetizationType
{
    /// <summary>
    /// Subscription-based streaming (e.g., Netflix, Disney+).
    /// </summary>
    [EnumValue("flatrate")]
    Flatrate,

    /// <summary>
    /// Free to watch (may include ads).
    /// </summary>
    [EnumValue("free")]
    Free,

    /// <summary>
    /// Ad-supported streaming.
    /// </summary>
    [EnumValue("ads")]
    Ads,

    /// <summary>
    /// Available for rental.
    /// </summary>
    [EnumValue("rent")]
    Rent,

    /// <summary>
    /// Available for purchase.
    /// </summary>
    [EnumValue("buy")]
    Buy
}
