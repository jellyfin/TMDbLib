namespace TMDbLib.Objects.TvShows
{
    /// <summary>
    /// Represents the type of episode grouping for a TV show.
    /// </summary>
    public enum TvGroupType
    {
        /// <summary>
        /// Episodes grouped by original air date.
        /// </summary>
        OriginalAirDate = 1,

        /// <summary>
        /// Episodes grouped by absolute numbering.
        /// </summary>
        Absolute = 2,

        /// <summary>
        /// Episodes grouped by DVD release order.
        /// </summary>
        DVD = 3,

        /// <summary>
        /// Episodes grouped by digital release order.
        /// </summary>
        Digital = 4,

        /// <summary>
        /// Episodes grouped by story arc.
        /// </summary>
        StoryArc = 5,

        /// <summary>
        /// Episodes grouped by production order.
        /// </summary>
        Production = 6,

        /// <summary>
        /// Episodes grouped by TV broadcast order.
        /// </summary>
        TV = 7
    }
}
