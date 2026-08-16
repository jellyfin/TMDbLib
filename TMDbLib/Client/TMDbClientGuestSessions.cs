using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    private async Task<SearchContainer<T>?> GetGuestSessionRatedInternal<T>(string path, string? language, int page, AccountSortBy sortBy, SortOrder sortOrder, CancellationToken cancellationToken)
    {
        RequireSessionId(SessionType.GuestSession);

        var request = _client.Create(path);

        if (page > 0)
        {
            request.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (!string.IsNullOrEmpty(language))
        {
            request.AddParameter("language", language);
        }

        if (sortBy != AccountSortBy.Undefined)
        {
            // TMDb expects the suffix on sort_by itself (e.g. created_at.asc). It does NOT
            // accept a separate sort_order query parameter for guest session endpoints.
            var direction = sortOrder == SortOrder.Descending ? "desc" : "asc";
            request.AddParameter("sort_by", $"{sortBy.GetDescription()}.{direction}");
        }

        AddSessionId(request, SessionType.GuestSession, ParameterType.UrlSegment);

        var resp = await request.GetOfT<SearchContainer<T>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the movies rated by the current guest session.
    /// </summary>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rated movies.</returns>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest session is set.</exception>
    public async Task<SearchContainer<SearchMovieWithRating>?> GetGuestSessionRatedMoviesAsync(int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetGuestSessionRatedMoviesAsync(DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the movies rated by the current guest session in a specific language.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rated movies.</returns>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest session is set.</exception>
    public async Task<SearchContainer<SearchMovieWithRating>?> GetGuestSessionRatedMoviesAsync(string? language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetGuestSessionRatedMoviesAsync(language, page, AccountSortBy.Undefined, SortOrder.Undefined, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the movies rated by the current guest session, sorted.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="sortOrder">The sort direction.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rated movies.</returns>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest session is set.</exception>
    public async Task<SearchContainer<SearchMovieWithRating>?> GetGuestSessionRatedMoviesAsync(string? language, int page, AccountSortBy sortBy, SortOrder sortOrder, CancellationToken cancellationToken = default)
    {
        return await GetGuestSessionRatedInternal<SearchMovieWithRating>("guest_session/{guest_session_id}/rated/movies", language, page, sortBy, sortOrder, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the TV shows rated by the current guest session.
    /// </summary>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rated TV shows.</returns>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest session is set.</exception>
    public async Task<SearchContainer<SearchTvShowWithRating>?> GetGuestSessionRatedTvAsync(int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetGuestSessionRatedTvAsync(DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the TV shows rated by the current guest session in a specific language.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rated TV shows.</returns>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest session is set.</exception>
    public async Task<SearchContainer<SearchTvShowWithRating>?> GetGuestSessionRatedTvAsync(string? language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetGuestSessionRatedTvAsync(language, page, AccountSortBy.Undefined, SortOrder.Undefined, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the TV shows rated by the current guest session, sorted.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="sortOrder">The sort direction.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rated TV shows.</returns>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest session is set.</exception>
    public async Task<SearchContainer<SearchTvShowWithRating>?> GetGuestSessionRatedTvAsync(string? language, int page, AccountSortBy sortBy, SortOrder sortOrder, CancellationToken cancellationToken = default)
    {
        return await GetGuestSessionRatedInternal<SearchTvShowWithRating>("guest_session/{guest_session_id}/rated/tv", language, page, sortBy, sortOrder, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the TV episodes rated by the current guest session.
    /// </summary>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rated TV episodes.</returns>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest session is set.</exception>
    public async Task<SearchContainer<TvEpisodeWithRating>?> GetGuestSessionRatedTvEpisodesAsync(int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetGuestSessionRatedTvEpisodesAsync(DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the TV episodes rated by the current guest session in a specific language.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rated TV episodes.</returns>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest session is set.</exception>
    public async Task<SearchContainer<TvEpisodeWithRating>?> GetGuestSessionRatedTvEpisodesAsync(string? language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetGuestSessionRatedTvEpisodesAsync(language, page, AccountSortBy.Undefined, SortOrder.Undefined, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the TV episodes rated by the current guest session, sorted.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="sortOrder">The sort direction.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rated TV episodes.</returns>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest session is set.</exception>
    public async Task<SearchContainer<TvEpisodeWithRating>?> GetGuestSessionRatedTvEpisodesAsync(string? language, int page, AccountSortBy sortBy, SortOrder sortOrder, CancellationToken cancellationToken = default)
    {
        return await GetGuestSessionRatedInternal<TvEpisodeWithRating>("guest_session/{guest_session_id}/rated/tv/episodes", language, page, sortBy, sortOrder, cancellationToken).ConfigureAwait(false);
    }
}
