using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents credits for a TV episode including guest stars.
/// </summary>
public class CreditsWithGuestStars : Credits
{
    /// <summary>
    /// Gets or sets the list of guest stars for the episode.
    /// </summary>
    [JsonProperty("guest_stars")]
    public List<Cast>? GuestStars { get; set; }
}
