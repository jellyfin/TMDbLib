using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Episode credits including guest stars.
/// </summary>
public class CreditsWithGuestStars : Credits
{
    /// <summary>
    /// Gets or sets the guest stars.
    /// </summary>
    [JsonPropertyName("guest_stars")]
    public List<Cast>? GuestStars { get; set; }
}
