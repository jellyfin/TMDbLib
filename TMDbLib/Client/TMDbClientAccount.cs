using TMDbLib.Utilities;
using System.Globalization;
using System.Threading.Tasks;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Change the favorite status of a specific movie. Either make the movie a favorite or remove that status depending on the supplied boolean value.
        /// </summary>
        /// <param name="mediaType">The type of media to influence</param>
        /// <param name="mediaId">The id of the movie/tv show to influence</param>
        /// <param name="isFavorite">True if you want the specified movie to be marked as favorite, false if not</param>
        /// <returns>True if the the movie's favorite status was successfully updated, false if not</returns>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<bool> AccountChangeFavoriteStatusAsync(MediaType mediaType, int mediaId, bool isFavorite)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest request =  _client.Create("account/{accountId}/favorite") ;
            request.AddUrlSegment("accountId", ActiveAccount.Id.ToString(CultureInfo.InvariantCulture));
            request.SetBody(new { media_type = mediaType.GetDescription(), media_id = mediaId, favorite = isFavorite });
            AddSessionId(request, SessionType.UserSession);

            PostReply response = await request.ExecutePost<PostReply>().ConfigureAwait(false);

            // status code 1 = "Success" - Returned when adding a movie as favorite for the first time
            // status code 13 = "The item/record was deleted successfully" - When removing an item as favorite, no matter if it exists or not
            // status code 12 = "The item/record was updated successfully" - Used when an item is already marked as favorite and trying to do so doing again
            return response.StatusCode == 1 || response.StatusCode == 12 || response.StatusCode == 13;
        }

        /// <summary>
        /// Change the state of a specific movie on the users watchlist. Either add the movie to the list or remove it, depending on the specified boolean value.
        /// </summary>
        /// <param name="mediaType">The type of media to influence</param>
        /// <param name="mediaId">The id of the movie/tv show to influence</param>
        /// <param name="isOnWatchlist">True if you want the specified movie to be part of the watchlist, false if not</param>
        /// <returns>True if the the movie's status on the watchlist was successfully updated, false if not</returns>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<bool> AccountChangeWatchlistStatusAsync(MediaType mediaType, int mediaId, bool isOnWatchlist)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest request =  _client.Create("account/{accountId}/watchlist");
            request.AddUrlSegment("accountId", ActiveAccount.Id.ToString(CultureInfo.InvariantCulture));
            request.SetBody(new { media_type = mediaType.GetDescription(), media_id = mediaId, watchlist = isOnWatchlist });
            AddSessionId(request, SessionType.UserSession);

            PostReply response = await request.ExecutePost<PostReply>().ConfigureAwait(false);

            // status code 1 = "Success"
            // status code 13 = "The item/record was deleted successfully" - When removing an item from the watchlist, no matter if it exists or not
            // status code 12 = "The item/record was updated successfully" - Used when an item is already on the watchlist and trying to add it again
            return response.StatusCode == 1 || response.StatusCode == 12 || response.StatusCode == 13;
        }

        /// <summary>
        /// Will retrieve the details of the account associated with the current session id
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<AccountDetails> AccountGetDetailsAsync()
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest request = _client.Create("account");
            AddSessionId(request, SessionType.UserSession);

            AccountDetails response = await request.ExecuteGet<AccountDetails>().ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Get a list of all the movies marked as favorite by the current user
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<SearchContainer<SearchMovie>> AccountGetFavoriteMoviesAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string language = null)
        {
            return await GetAccountList<SearchMovie>(page, sortBy, sortOrder, language, AccountListsMethods.FavoriteMovies).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of all the tv shows marked as favorite by the current user
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<SearchContainer<SearchTv>> AccountGetFavoriteTvAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string language = null)
        {
            return await GetAccountList<SearchTv>(page, sortBy, sortOrder, language, AccountListsMethods.FavoriteTv).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve all lists associated with the provided account id
        /// This can be lists that were created by the user or lists marked as favorite
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<SearchContainer<AccountList>> AccountGetListsAsync(int page = 1, string language = null)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest request = _client.Create("account/{accountId}/lists");
            request.AddUrlSegment("accountId", ActiveAccount.Id.ToString(CultureInfo.InvariantCulture));
            AddSessionId(request, SessionType.UserSession);

            if (page > 1)
            {
                request.AddQueryString("page", page.ToString());
            }

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                request.AddQueryString("language", language);

            SearchContainer<AccountList> response = await request.ExecuteGet<SearchContainer<AccountList>>().ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Get a list of all the movies on the current users match list
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<SearchContainer<SearchMovie>> AccountGetMovieWatchlistAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string language = null)
        {
            return await GetAccountList<SearchMovie>(page, sortBy, sortOrder, language, AccountListsMethods.MovieWatchlist).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of all the movies rated by the current user
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<SearchContainer<SearchMovie>> AccountGetRatedMoviesAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string language = null)
        {
            return await GetAccountList<SearchMovie>(page, sortBy, sortOrder, language, AccountListsMethods.RatedMovies).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of all the tv show episodes rated by the current user
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<SearchContainer<AccountSearchTvEpisode>> AccountGetRatedTvShowEpisodesAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string language = null)
        {
            return await GetAccountList<AccountSearchTvEpisode>(page, sortBy, sortOrder, language, AccountListsMethods.RatedTvEpisodes).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of all the tv shows rated by the current user
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<SearchContainer<AccountSearchTv>> AccountGetRatedTvShowsAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string language = null)
        {
            return await GetAccountList<AccountSearchTv>(page, sortBy, sortOrder, language, AccountListsMethods.RatedTv).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of all the tv shows on the current users match list
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<SearchContainer<SearchTv>> AccountGetTvWatchlistAsync(
            int page = 1,
            AccountSortBy sortBy = AccountSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string language = null)
        {
            return await GetAccountList<SearchTv>(page, sortBy, sortOrder, language, AccountListsMethods.TvWatchlist).ConfigureAwait(false);
        }

        private async Task<SearchContainer<T>> GetAccountList<T>(int page, AccountSortBy sortBy, SortOrder sortOrder, string language, AccountListsMethods method)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest request =  _client.Create("account/{accountId}/" + method.GetDescription());
            request.AddUrlSegment("accountId", ActiveAccount.Id.ToString(CultureInfo.InvariantCulture));
            AddSessionId(request, SessionType.UserSession);

            if (page > 1)
                request.AddParameter("page", page.ToString());

            if (sortBy != AccountSortBy.Undefined)
                request.AddParameter("sort_by", sortBy.GetDescription());

            if (sortOrder != SortOrder.Undefined)
                request.AddParameter("sort_order", sortOrder.GetDescription());

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            SearchContainer<T> response = await request.ExecuteGet<SearchContainer<T>>().ConfigureAwait(false);

            return response;
        }

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
    }
}
