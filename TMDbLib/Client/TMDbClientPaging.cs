using System.Collections.Generic;
using System.Threading;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities;

namespace TMDbLib.Client;

/// <summary>
/// Streaming counterparts of the page-based endpoints. Each of these fetches one page per 20 items
/// as the sequence is consumed, so enumerating a large result set is a lot of requests - mind the
/// rate limit. Any other paged endpoint can be enumerated the same way with
/// <see cref="PagedResults.EnumerateAllAsync{T}"/>.
/// </summary>
public partial class TMDbClient
{
    /// <summary>
    /// Enumerates every item of a list, page by page. TMDb serves the items of a list 20 at a time,
    /// so a longer list needs more than one request.
    /// </summary>
    /// <param name="listId">The id of the list.</param>
    /// <param name="language">The ISO 639-1 language code (e.g. en, it, es).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The items of the list.</returns>
    public IAsyncEnumerable<TmdbEntity> GetListItemsAsync(int listId, string? language = null, CancellationToken cancellationToken = default)
        => PagedResults.EnumerateAllAsync<TmdbEntity>(
            async (page, token) => await GetListAsync(listId, language, page, token).ConfigureAwait(false),
            cancellationToken);

    /// <summary>
    /// Enumerates every movie matching a search query.
    /// </summary>
    /// <param name="query">The query to search for.</param>
    /// <param name="language">The ISO 639-1 language code (e.g. en, it, es).</param>
    /// <param name="includeAdult">Whether to include adult titles.</param>
    /// <param name="year">Restricts the results to a release year.</param>
    /// <param name="region">The ISO 3166-1 region code.</param>
    /// <param name="primaryReleaseYear">Restricts the results to a primary release year.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching movies.</returns>
    public IAsyncEnumerable<SearchMovie> SearchMovieAllAsync(
        string query,
        string? language = null,
        bool includeAdult = false,
        int year = 0,
        string? region = null,
        int primaryReleaseYear = 0,
        CancellationToken cancellationToken = default)
        => PagedResults.EnumerateAllAsync<SearchMovie>(
            async (page, token) => await SearchMovieAsync(query, language, page, includeAdult, year, region, primaryReleaseYear, token).ConfigureAwait(false),
            cancellationToken);

    /// <summary>
    /// Enumerates every TV show matching a search query.
    /// </summary>
    /// <param name="query">The query to search for.</param>
    /// <param name="language">The ISO 639-1 language code (e.g. en, it, es).</param>
    /// <param name="includeAdult">Whether to include adult titles.</param>
    /// <param name="firstAirDateYear">Restricts the results to a year of first airing.</param>
    /// <param name="year">Restricts the results to a year.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching TV shows.</returns>
    public IAsyncEnumerable<SearchTv> SearchTvShowAllAsync(
        string query,
        string? language = null,
        bool includeAdult = false,
        int firstAirDateYear = 0,
        int year = 0,
        CancellationToken cancellationToken = default)
        => PagedResults.EnumerateAllAsync<SearchTv>(
            async (page, token) => await SearchTvShowAsync(query, language, page, includeAdult, firstAirDateYear, year, token).ConfigureAwait(false),
            cancellationToken);

    /// <summary>
    /// Enumerates every movie, TV show and person matching a search query.
    /// </summary>
    /// <param name="query">The query to search for.</param>
    /// <param name="language">The ISO 639-1 language code (e.g. en, it, es).</param>
    /// <param name="includeAdult">Whether to include adult titles.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching entities.</returns>
    public IAsyncEnumerable<TmdbEntity> SearchMultiAllAsync(
        string query,
        string? language = null,
        bool includeAdult = false,
        CancellationToken cancellationToken = default)
        => PagedResults.EnumerateAllAsync<TmdbEntity>(
            async (page, token) => await SearchMultiAsync(query, language, page, includeAdult, token).ConfigureAwait(false),
            cancellationToken);

    /// <summary>
    /// Enumerates every collection matching a search query.
    /// </summary>
    /// <param name="query">The query to search for.</param>
    /// <param name="language">The ISO 639-1 language code (e.g. en, it, es).</param>
    /// <param name="includeAdult">Whether to include adult titles.</param>
    /// <param name="region">The ISO 3166-1 region code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching collections.</returns>
    public IAsyncEnumerable<SearchCollection> SearchCollectionAllAsync(
        string query,
        string? language = null,
        bool includeAdult = false,
        string? region = null,
        CancellationToken cancellationToken = default)
        => PagedResults.EnumerateAllAsync<SearchCollection>(
            async (page, token) => await SearchCollectionAsync(query, language, page, includeAdult, region, token).ConfigureAwait(false),
            cancellationToken);

    /// <summary>
    /// Enumerates every person matching a search query.
    /// </summary>
    /// <param name="query">The query to search for.</param>
    /// <param name="language">The ISO 639-1 language code (e.g. en, it, es).</param>
    /// <param name="includeAdult">Whether to include adult titles.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching people.</returns>
    public IAsyncEnumerable<SearchPerson> SearchPersonAllAsync(
        string query,
        string? language = null,
        bool includeAdult = false,
        CancellationToken cancellationToken = default)
        => PagedResults.EnumerateAllAsync<SearchPerson>(
            async (page, token) => await SearchPersonAsync(query, language, page, includeAdult, token).ConfigureAwait(false),
            cancellationToken);

    /// <summary>
    /// Enumerates every movie similar to a movie.
    /// </summary>
    /// <param name="movieId">The id of the movie.</param>
    /// <param name="language">The ISO 639-1 language code (e.g. en, it, es).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The similar movies.</returns>
    public IAsyncEnumerable<SearchMovie> GetMovieSimilarAllAsync(int movieId, string? language = null, CancellationToken cancellationToken = default)
        => PagedResults.EnumerateAllAsync<SearchMovie>(
            async (page, token) => await GetMovieSimilarAsync(movieId, language, page, token).ConfigureAwait(false),
            cancellationToken);

    /// <summary>
    /// Enumerates every movie recommended for a movie.
    /// </summary>
    /// <param name="movieId">The id of the movie.</param>
    /// <param name="language">The ISO 639-1 language code (e.g. en, it, es).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The recommended movies.</returns>
    public IAsyncEnumerable<SearchMovie> GetMovieRecommendationsAllAsync(int movieId, string? language = null, CancellationToken cancellationToken = default)
        => PagedResults.EnumerateAllAsync<SearchMovie>(
            async (page, token) => await GetMovieRecommendationsAsync(movieId, language, page, token).ConfigureAwait(false),
            cancellationToken);

    /// <summary>
    /// Enumerates every TV show similar to a TV show.
    /// </summary>
    /// <param name="tvShowId">The id of the TV show.</param>
    /// <param name="language">The ISO 639-1 language code (e.g. en, it, es).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The similar TV shows.</returns>
    public IAsyncEnumerable<SearchTv> GetTvShowSimilarAllAsync(int tvShowId, string? language = null, CancellationToken cancellationToken = default)
        => PagedResults.EnumerateAllAsync<SearchTv>(
            async (page, token) => await GetTvShowSimilarAsync(tvShowId, language, page, token).ConfigureAwait(false),
            cancellationToken);

    /// <summary>
    /// Enumerates every TV show recommended for a TV show.
    /// </summary>
    /// <param name="tvShowId">The id of the TV show.</param>
    /// <param name="language">The ISO 639-1 language code (e.g. en, it, es).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The recommended TV shows.</returns>
    public IAsyncEnumerable<SearchTv> GetTvShowRecommendationsAllAsync(int tvShowId, string? language = null, CancellationToken cancellationToken = default)
        => PagedResults.EnumerateAllAsync<SearchTv>(
            async (page, token) => await GetTvShowRecommendationsAsync(tvShowId, language, page, token).ConfigureAwait(false),
            cancellationToken);
}
