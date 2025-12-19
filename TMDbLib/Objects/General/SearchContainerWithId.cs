namespace TMDbLib.Objects.General
{
    /// <summary>
    /// Represents a search container that includes an ID property.
    /// </summary>
    /// <typeparam name="T">The type of items contained in the search results.</typeparam>
    public class SearchContainerWithId<T> : SearchContainer<T>
    {
        /// <summary>
        /// Gets or sets the TMDb ID.
        /// </summary>
        public int Id { get; set; }
    }
}
