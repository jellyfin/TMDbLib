namespace TMDbLib.Objects.Movies
{
    /// <summary>
    /// Represents the type of movie release.
    /// </summary>
    public enum ReleaseDateType
    {
        /// <summary>
        /// Premiere release.
        /// </summary>
        Premiere = 1,

        /// <summary>
        /// Theatrical limited release.
        /// </summary>
        TheatricalLimited = 2,

        /// <summary>
        /// Theatrical release.
        /// </summary>
        Theatrical = 3,

        /// <summary>
        /// Digital release.
        /// </summary>
        Digital = 4,

        /// <summary>
        /// Physical release.
        /// </summary>
        Physical = 5,

        /// <summary>
        /// TV release.
        /// </summary>
        TV = 6
    }
}
