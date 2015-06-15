using System;
using System.ComponentModel;
using System.Globalization;
using RestSharp;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Will retrieve the details of the account associated with the current session id
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public AccountDetails AccountGetDetails()
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest request = new RestRequest("account");
            request.AddParameter("session_id", SessionId);

            IRestResponse<AccountDetails> response = _client.Get<AccountDetails>(request);

            return response.Data;
        }

        /// <summary>
        /// Retrieve all lists associated with the provided account id
        /// This can be lists that were created by the user or lists marked as favorite
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public SearchContainer<List> AccountGetLists(int page = 1, string language = null)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest request = new RestRequest("account/{accountId}/lists");
            request.AddUrlSegment("accountId", ActiveAccount.Id.ToString(CultureInfo.InvariantCulture));
            request.AddParameter("session_id", SessionId);

            if (page > 1)
                request.AddParameter("page", page);

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            IRestResponse<SearchContainer<List>> response = _client.Get<SearchContainer<List>>(request);

            return response.Data;
        }

        /// <summary>
        /// Change the favorite status of a specific movie. Either make the movie a favorite or remove that status depending on the supplied boolean value.
        /// </summary>
        /// <param name="mediaType">The type of media to influence</param>
        /// <param name="mediaId">The id of the movie/tv show to influence</param>
        /// <param name="isFavorite">True if you want the specified movie to be marked as favorite, false if not</param>
        /// <returns>True if the the movie's favorite status was successfully updated, false if not</returns>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public bool AccountChangeFavoriteStatus(MediaType mediaType, int mediaId, bool isFavorite)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest request = new RestRequest("account/{accountId}/favorite") { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("accountId", ActiveAccount.Id.ToString(CultureInfo.InvariantCulture));
            request.AddParameter("session_id", SessionId, ParameterType.QueryString);
            request.AddBody(new { media_type = mediaType.GetDescription(), media_id = mediaId, favorite = isFavorite });

            IRestResponse<PostReply> response = _client.Post<PostReply>(request);

            // status code 1 = "Success" - Returned when adding a movie as favorite for the first time
            // status code 13 = "The item/record was deleted successfully" - When removing an item as favorite, no matter if it exists or not
            // status code 12 = "The item/record was updated successfully" - Used when an item is already marked as favorite and trying to do so doing again
            return response.Data != null && (response.Data.StatusCode == 1 || response.Data.StatusCode == 12 || response.Data.StatusCode == 13);
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
        public bool AccountChangeWatchlistStatus(MediaType mediaType, int mediaId, bool isOnWatchlist)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest request = new RestRequest("account/{accountId}/watchlist") { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("accountId", ActiveAccount.Id.ToString(CultureInfo.InvariantCulture));
            request.AddParameter("session_id", SessionId, ParameterType.QueryString);
            request.AddBody(new { media_type = mediaType.GetDescription(), media_id = mediaId, watchlist = isOnWatchlist });

            IRestResponse<PostReply> response = _client.Post<PostReply>(request);

            // status code 1 = "Success"
            // status code 13 = "The item/record was deleted successfully" - When removing an item from the watchlist, no matter if it exists or not
            // status code 12 = "The item/record was updated successfully" - Used when an item is already on the watchlist and trying to add it again
            return response.Data != null && (response.Data.StatusCode == 1 || response.Data.StatusCode == 12 || response.Data.StatusCode == 13);
        }

        /// <summary>
        /// Get a list of all the movies marked as favorite by the current user
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public SearchContainer<SearchMovie> AccountGetFavoriteMovies(
            int page = 1,
            AccountMovieSortBy sortBy = AccountMovieSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string language = null)
        {
            return GetAccountList<SearchMovie>(page, sortBy, sortOrder, language, AccountListsMethods.FavoriteMovies);
        }

        /// <summary>
        /// Get a list of all the tv shows marked as favorite by the current user
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public SearchContainer<SearchTv> AccountGetFavoriteTv(
            int page = 1,
            AccountMovieSortBy sortBy = AccountMovieSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string language = null)
        {
            return GetAccountList<SearchTv>(page, sortBy, sortOrder, language, AccountListsMethods.FavoriteTv);
        }

        /// <summary>
        /// Get a list of all the movies on the current users match list
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public SearchContainer<SearchMovie> AccountGetMovieWatchlist(
            int page = 1,
            AccountMovieSortBy sortBy = AccountMovieSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string language = null)
        {
            return GetAccountList<SearchMovie>(page, sortBy, sortOrder, language, AccountListsMethods.MovieWatchlist);
        }

        public SearchContainer<SearchTv> AccountGetTvWatchlist(
            int page = 1,
            AccountMovieSortBy sortBy = AccountMovieSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string language = null)
        {
            return GetAccountList<SearchTv>(page, sortBy, sortOrder, language, AccountListsMethods.TvWatchlist);
        }

        /// <summary>
        /// Get a list of all the movies rated by the current user
        /// </summary>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public SearchContainer<SearchMovie> AccountGetRatedMovies(
            int page = 1,
            AccountMovieSortBy sortBy = AccountMovieSortBy.Undefined,
            SortOrder sortOrder = SortOrder.Undefined,
            string language = null)
        {
            return GetAccountList<SearchMovie>(page, sortBy, sortOrder, language, AccountListsMethods.RatedMovies);
        }

        private SearchContainer<T> GetAccountList<T>(int page, AccountMovieSortBy sortBy, SortOrder sortOrder, string language, AccountListsMethods method)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest request = new RestRequest("account/{accountId}/{method}");
            request.AddUrlSegment("accountId", ActiveAccount.Id.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("method", method.GetDescription());
            request.AddParameter("session_id", SessionId);

            if (page > 1)
                request.AddParameter("page", page);

            if (sortBy != AccountMovieSortBy.Undefined)
                request.AddParameter("sort_by", sortBy.GetDescription());

            if (sortOrder != SortOrder.Undefined)
                request.AddParameter("sort_order", sortOrder.GetDescription());

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            IRestResponse<SearchContainer<T>> response = _client.Get<SearchContainer<T>>(request);

            return response.Data;
        }

        private enum AccountListsMethods
        {
            [Description("favorite/movies")]
            FavoriteMovies,
            [Description("favorite/tv")]
            FavoriteTv,
            [Description("rated_movies")]
            RatedMovies,
            [Description("watchlist/movies")]
            MovieWatchlist,
            [Description("watchlist/tv")]
            TvWatchlist,
        }
    }
}
