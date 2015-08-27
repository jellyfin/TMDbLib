using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TMDbLib.Utilities;
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Retrieve a tv Show by id.
        /// </summary>
        /// <param name="id">TMDb id of the tv show to retrieve.</param>
        /// <param name="extraMethods">Enum flags indicating any additional data that should be fetched in the same request.</param>
        /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es </param>
        /// <returns>The requested Tv Show</returns>
        public async Task<TvShow> GetTvShow(int id, TvShowMethods extraMethods = TvShowMethods.Undefined, string language = null)
        {
            if (extraMethods.HasFlag(TvShowMethods.AccountStates))
                RequireSessionId(SessionType.UserSession);

            RestRequest request = new RestRequest("tv/{id}");
            request.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));

            if (extraMethods.HasFlag(TvShowMethods.AccountStates))
                AddSessionId(request, SessionType.UserSession);

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(TvShowMethods))
                                             .OfType<TvShowMethods>()
                                             .Except(new[] { TvShowMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                request.AddParameter("append_to_response", appends);

            IRestResponse<TvShow> response = await _client.ExecuteGetTaskAsync<TvShow>(request).ConfigureAwait(false);

            // No data to patch up so return
            if (response.Data == null)
                return null;

            // Patch up data, so that the end user won't notice that we share objects between request-types.
            if (response.Data.Translations != null)
                response.Data.Translations.Id = id;

            if (response.Data.AccountStates != null)
            {
                response.Data.AccountStates.Id = response.Data.Id;
                // Do some custom deserialization, since TMDb uses a property that changes type we can't use automatic deserialization
                CustomDeserialization.DeserializeAccountStatesRating(response.Data.AccountStates, response.Content);
            }

            return response.Data;
        }

        /// <summary>
        /// Get the list of popular TV shows. This list refreshes every day.
        /// </summary>
        /// <returns>
        /// Returns the basic information about a tv show.
        /// For additional data use the main GetTvShow method using the tv show id as parameter.
        /// </returns>
        public async Task<SearchContainer<SearchTv>> GetTvShowPopular(int page = -1, string language = null)
        {
            return await GetTvShowList(page, language, "popular");
        }

        [Obsolete("Use GetTvShowPopular")]
        public async Task<SearchContainer<SearchTv>> GetTvShowsPopular(int page = -1, string language = null)
        {
            return await GetTvShowPopular(page, language);
        }

        /// <summary>
        /// Get the list of top rated TV shows. By default, this list will only include TV shows that have 2 or more votes. This list refreshes every day.
        /// </summary>
        /// <returns>
        /// Returns the basic information about a tv show.
        /// For additional data use the main GetTvShow method using the tv show id as parameter
        /// </returns>
        public async Task<SearchContainer<SearchTv>> GetTvShowTopRated(int page = -1, string language = null)
        {
            return await GetTvShowList(page, language, "top_rated");
        }

        [Obsolete("Use GetTvShowTopRated")]
        public async Task<SearchContainer<SearchTv>> GetTvShowsTopRated(int page = -1, string language = null)
        {
            return await GetTvShowTopRated(page, language);
        }

        private async Task<SearchContainer<SearchTv>> GetTvShowList(int page, string language, string tvShowListType)
        {
            RestRequest req = new RestRequest("tv/" + tvShowListType);

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page);

            IRestResponse<SearchContainer<SearchTv>> response = await _client.ExecuteGetTaskAsync<SearchContainer<SearchTv>>(req).ConfigureAwait(false);

            return response.Data;
        }

        /// <summary>
        /// Returns a credits object for the tv show associated with the provided TMDb id.
        /// </summary>
        /// <param name="id">The TMDb id of the target tv show.</param>
        /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es </param>
        public async Task<Credits> GetTvShowCredits(int id, string language = null)
        {
            return await GetTvShowMethod<Credits>(id, TvShowMethods.Credits, dateFormat: "yyyy-MM-dd", language: language);
        }

        /// <summary>
        /// Retrieves all images all related to the specified tv show.
        /// </summary>
        /// <param name="id">The TMDb id of the target tv show.</param>
        /// <param name="language">
        /// If specified the api will attempt to return a localized result. ex: en,it,es.
        /// For images this means that the image might contain language specifc text
        /// </param>
        public async Task<ImagesWithId> GetTvShowImages(int id, string language = null)
        {
            return await GetTvShowMethod<ImagesWithId>(id, TvShowMethods.Images, language: language);
        }

        /// <summary>
        /// Returns an object that contains all known exteral id's for the tv show related to the specified TMDB id.
        /// </summary>
        /// <param name="id">The TMDb id of the target tv show.</param>
        public async Task<ExternalIds> GetTvShowExternalIds(int id)
        {
            return await GetTvShowMethod<ExternalIds>(id, TvShowMethods.ExternalIds);
        }

        public async Task<SearchContainer<SearchTv>> GetTvShowSimilar(int id, int page = 0)
        {
            return await GetTvShowSimilar(id, DefaultLanguage, page);
        }

        public async Task<SearchContainer<SearchTv>> GetTvShowSimilar(int id, string language, int page)
        {
            return await GetTvShowMethod<SearchContainer<SearchTv>>(id, TvShowMethods.Similar, language: language, page: page);
        }

        public async Task<ResultContainer<ContentRating>> GetTvShowContentRatings(int id)
        {
            return await GetTvShowMethod<ResultContainer<ContentRating>>(id, TvShowMethods.ContentRatings);
        }

        public async Task<ResultContainer<AlternativeTitle>> GetTvShowAlternativeTitles(int id)
        {
            return await GetTvShowMethod<ResultContainer<AlternativeTitle>>(id, TvShowMethods.AlternativeTitles);
        }

        public async Task<ResultContainer<Keyword>> GetTvShowKeywords(int id)
        {
            return await GetTvShowMethod<ResultContainer<Keyword>>(id, TvShowMethods.Keywords);
        }

        public async Task<ResultContainer<Video>> GetTvShowVideos(int id)
        {
            return await GetTvShowMethod<ResultContainer<Video>>(id, TvShowMethods.Videos);
        }

        public async Task<TranslationsContainer> GetTvShowTranslations(int id)
        {
            return await GetTvShowMethod<TranslationsContainer>(id, TvShowMethods.Translations);
        }

        public async Task<ChangesContainer> GetTvShowChanges(int id)
        {
            return await GetTvShowMethod<ChangesContainer>(id, TvShowMethods.Changes);
        }

        public async Task<TvShow> GetLatestTvShow()
        {
            RestRequest req = new RestRequest("tv/latest");

            IRestResponse<TvShow> resp = await _client.ExecuteGetTaskAsync<TvShow>(req);

            return resp.Data;
        }

        /// <summary>
        /// Fetches a dynamic list of TV Shows
        /// </summary>
        /// <param name="list">Type of list to fetch</param>
        /// <param name="page">Page</param>
        /// <param name="timezone">Only relevant for list type AiringToday</param>
        /// <returns></returns>
        public async Task<SearchContainer<TvShow>> GetTvShowList(TvShowListType list, int page = 0, string timezone = null)
        {
            return await GetTvShowList(list, DefaultLanguage, page, timezone);
        }

        /// <summary>
        /// Fetches a dynamic list of TV Shows
        /// </summary>
        /// <param name="list">Type of list to fetch</param>
        /// <param name="language">Language</param>
        /// <param name="page">Page</param>
        /// <param name="timezone">Only relevant for list type AiringToday</param>
        /// <returns></returns>
        public async Task<SearchContainer<TvShow>> GetTvShowList(TvShowListType list, string language, int page = 0, string timezone = null)
        {
            RestRequest req = new RestRequest("tv/{method}");
            req.AddUrlSegment("method", list.GetDescription());

            if (page > 0)
                req.AddParameter("page", page);

            if (!string.IsNullOrEmpty(timezone))
                req.AddParameter("timezone", timezone);

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            IRestResponse<SearchContainer<TvShow>> resp = await _client.ExecuteGetTaskAsync<SearchContainer<TvShow>>(req);

            return resp.Data;
        }

        private async Task<T> GetTvShowMethod<T>(int id, TvShowMethods tvShowMethod, string dateFormat = null, string language = null, int page = 0) where T : new()
        {
            RestRequest req = new RestRequest("tv/{id}/{method}");
            req.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("method", tvShowMethod.GetDescription());

            if (dateFormat != null)
                req.DateFormat = dateFormat;

            if (page > 0)
                req.AddParameter("page", page);

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            IRestResponse<T> resp = await _client.ExecuteGetTaskAsync<T>(req).ConfigureAwait(false);

            return resp.Data;
        }

        /// <summary>
        /// Retrieves all information for a specific tv show in relation to the current user account
        /// </summary>
        /// <param name="tvShowId">The id of the tv show to get the account states for</param>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<AccountState> GetTvShowAccountState(int tvShowId)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest request = new RestRequest("tv/{tvShowId}/{method}");
            request.AddUrlSegment("tvShowId", tvShowId.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("method", TvShowMethods.AccountStates.GetDescription());
            AddSessionId(request, SessionType.UserSession);

            IRestResponse<AccountState> response = await _client.ExecuteGetTaskAsync<AccountState>(request);

            // Do some custom deserialization, since TMDb uses a property that changes type we can't use automatic deserialization
            if (response.Data != null)
            {
                CustomDeserialization.DeserializeAccountStatesRating(response.Data, response.Content);
            }

            return response.Data;
        }

        /// <summary>
        /// Change the rating of a specified tv show.
        /// </summary>
        /// <param name="tvShowId">The id of the tv show to rate</param>
        /// <param name="rating">The rating you wish to assign to the specified tv show. Value needs to be between 0.5 and 10 and must use increments of 0.5. Ex. using 7.1 will not work and return false.</param>
        /// <returns>True if the the tv show's rating was successfully updated, false if not</returns>
        /// <remarks>Requires a valid guest or user session</remarks>
        /// <exception cref="GuestSessionRequiredException">Thrown when the current client object doens't have a guest or user session assigned.</exception>
        public async Task<bool> TvShowSetRating(int tvShowId, double rating)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest request = new RestRequest("tv/{tvShowId}/rating") { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("tvShowId", tvShowId.ToString(CultureInfo.InvariantCulture));
            AddSessionId(request);

            request.AddBody(new { value = rating });

            IRestResponse<PostReply> response = await _client.ExecutePostTaskAsync<PostReply>(request);

            // status code 1 = "Success"
            // status code 12 = "The item/record was updated successfully" - Used when an item was previously rated by the user
            return response.Data != null && (response.Data.StatusCode == 1 || response.Data.StatusCode == 12);
        }

        public async Task<bool> TvShowRemoveRating(int tvShowId)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest request = new RestRequest("tv/{tvShowId}/rating");
            request.AddUrlSegment("tvShowId", tvShowId.ToString(CultureInfo.InvariantCulture));
            AddSessionId(request);

            IRestResponse<PostReply> response = await _client.ExecuteDeleteTaskAsync<PostReply>(request);

            // status code 13 = "The item/record was deleted successfully."
            return response.Data != null && response.Data.StatusCode == 13;
        }
    }
}
