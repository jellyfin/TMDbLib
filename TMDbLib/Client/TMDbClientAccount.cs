using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    private enum AccountListsMethods
    {
        [EnumValue("favorite/movies")]
        FavoriteMovies,
        [EnumValue("favorite/tv")]
        FavoriteTv,
        [EnumValue("rated/movies")]
        RatedMovies,
        [EnumValue("rated/tv")]
        RatedTv,
        [EnumValue("rated/tv/episodes")]
        RatedTvEpisodes,
        [EnumValue("watchlist/movies")]
        MovieWatchlist,
        [EnumValue("watchlist/tv")]
        TvWatchlist,
    }

    private async Task<SearchContainer<T>?> GetAccountListInternal<T>(int page, AccountSortBy sortBy, SortOrder sortOrder, string? language, AccountListsMethods method, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        var request = _client.Create("account/{accountId}/" + method.GetDescription());
        request.AddUrlSegment("accountId", ActiveAccount!.Id.ToString(CultureInfo.InvariantCulture));
        AddSessionId(request, SessionType.UserSession);

        if (page > 1)
        {
            request.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (sortBy != AccountSortBy.Undefined)
        {
            // TMDb expects the suffix on sort_by itself (e.g. created_at.asc). It does NOT
            // accept a separate sort_order query parameter for account list endpoints.
            var direction = sortOrder == SortOrder.Descending ? "desc" : "asc";
            request.AddParameter("sort_by", $"{sortBy.GetDescription()}.{direction}");
        }

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            request.AddParameter("language", language);
        }

        var response = await request.GetOfT<SearchContainer<T>>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Marks or unmarks a media item as favorite.
    /// </summary>
    /// <param name="mediaType">The media type.</param>
    /// <param name="mediaId">The media id.</param>
    /// <param name="isFavorite">True to mark as favorite, false to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the favorite status was updated.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<bool> AccountChangeFavoriteStatusAsync(MediaType mediaType, int mediaId, bool isFavorite, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        var request = _client.Create("account/{accountId}/favorite");
        request.AddUrlSegment("accountId", ActiveAccount!.Id.ToString(CultureInfo.InvariantCulture));
        request.SetBody(new { media_type = mediaType.GetDescription(), media_id = mediaId, favorite = isFavorite });
        AddSessionId(request, SessionType.UserSession);

        var response = await request.PostOfT<PostReply>(cancellationToken).ConfigureAwait(false);

        // status code 1 = "Success" - Returned when adding a movie as favorite for the first time
        // status code 13 = "The item/record was deleted successfully" - When removing an item as favorite, no matter if it exists or not
        // status code 12 = "The item/record was updated successfully" - Used when an item is already marked as favorite and trying to do so doing again
        return response?.StatusCode == 1 || response?.StatusCode == 12 || response?.StatusCode == 13;
    }

    /// <summary>
    /// Adds or removes a media item from the user's watchlist.
    /// </summary>
    /// <param name="mediaType">The media type.</param>
    /// <param name="mediaId">The media id.</param>
    /// <param name="isOnWatchlist">True to add, false to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the watchlist status was updated.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<bool> AccountChangeWatchlistStatusAsync(MediaType mediaType, int mediaId, bool isOnWatchlist, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        var request = _client.Create("account/{accountId}/watchlist");
        request.AddUrlSegment("accountId", ActiveAccount!.Id.ToString(CultureInfo.InvariantCulture));
        request.SetBody(new { media_type = mediaType.GetDescription(), media_id = mediaId, watchlist = isOnWatchlist });
        AddSessionId(request, SessionType.UserSession);

        var response = await request.PostOfT<PostReply>(cancellationToken).ConfigureAwait(false);

        // status code 1 = "Success"
        // status code 13 = "The item/record was deleted successfully" - When removing an item from the watchlist, no matter if it exists or not
        // status code 12 = "The item/record was updated successfully" - Used when an item is already on the watchlist and trying to add it again
        return response?.StatusCode == 1 || response?.StatusCode == 12 || response?.StatusCode == 13;
    }

    /// <summary>
    /// Gets the account details for the current session.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The account details.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<AccountDetails?> AccountGetDetailsAsync(CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        var request = _client.Create("account");
        AddSessionId(request, SessionType.UserSession);

        var response = await request.GetOfT<AccountDetails>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Gets the current user's favorite movies.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortOrder">The sort order.</param>
    /// <param name="language">The language for localized results.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The favorite movies.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<SearchContainer<SearchMovie>?> AccountGetFavoriteMoviesAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string? language = null,
            CancellationToken cancellationToken = default)
    {
        return await GetAccountListInternal<SearchMovie>(page, sortBy, sortOrder, language, AccountListsMethods.FavoriteMovies, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the current user's favorite TV shows.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortOrder">The sort order.</param>
    /// <param name="language">The language for localized results.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The favorite TV shows.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<SearchContainer<SearchTv>?> AccountGetFavoriteTvAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string? language = null,
            CancellationToken cancellationToken = default)
    {
        return await GetAccountListInternal<SearchTv>(page, sortBy, sortOrder, language, AccountListsMethods.FavoriteTv, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets all lists created by or marked as favorite by the current user.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The account lists.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<SearchContainer<AccountList>?> AccountGetListsAsync(int page = 1, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        var request = _client.Create("account/{accountId}/lists");
        request.AddUrlSegment("accountId", ActiveAccount!.Id.ToString(CultureInfo.InvariantCulture));
        AddSessionId(request, SessionType.UserSession);

        if (page > 1)
        {
            request.AddQueryString("page", page.ToString(CultureInfo.InvariantCulture));
        }

        var response = await request.GetOfT<SearchContainer<AccountList>>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Gets the movies on the current user's watchlist.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortOrder">The sort order.</param>
    /// <param name="language">The language for localized results.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movies on the watchlist.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<SearchContainer<SearchMovie>?> AccountGetMovieWatchlistAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string? language = null,
            CancellationToken cancellationToken = default)
    {
        return await GetAccountListInternal<SearchMovie>(page, sortBy, sortOrder, language, AccountListsMethods.MovieWatchlist, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the movies rated by the current user.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortOrder">The sort order.</param>
    /// <param name="language">The language for localized results.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rated movies.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<SearchContainer<SearchMovieWithRating>?> AccountGetRatedMoviesAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string? language = null,
            CancellationToken cancellationToken = default)
    {
        return await GetAccountListInternal<SearchMovieWithRating>(page, sortBy, sortOrder, language, AccountListsMethods.RatedMovies, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the TV show episodes rated by the current user.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortOrder">The sort order.</param>
    /// <param name="language">The language for localized results.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rated TV show episodes.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<SearchContainer<AccountSearchTvEpisode>?> AccountGetRatedTvShowEpisodesAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string? language = null,
            CancellationToken cancellationToken = default)
    {
        return await GetAccountListInternal<AccountSearchTvEpisode>(page, sortBy, sortOrder, language, AccountListsMethods.RatedTvEpisodes, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the TV shows rated by the current user.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortOrder">The sort order.</param>
    /// <param name="language">The language for localized results.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The rated TV shows.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<SearchContainer<AccountSearchTv>?> AccountGetRatedTvShowsAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string? language = null,
            CancellationToken cancellationToken = default)
    {
        return await GetAccountListInternal<AccountSearchTv>(page, sortBy, sortOrder, language, AccountListsMethods.RatedTv, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the TV shows on the current user's watchlist.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortOrder">The sort order.</param>
    /// <param name="language">The language for localized results.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV shows on the watchlist.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<SearchContainer<SearchTv>?> AccountGetTvWatchlistAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string? language = null,
            CancellationToken cancellationToken = default)
    {
        return await GetAccountListInternal<SearchTv>(page, sortBy, sortOrder, language, AccountListsMethods.TvWatchlist, cancellationToken).ConfigureAwait(false);
    }
}
