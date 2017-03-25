using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TMDbLib.Rest;
using TMDbLib.Utilities;
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<TvShow> GetLatestTvShowAsync()
        {
            RestRequest req = _client.Create("tv/latest");

            RestResponse<TvShow> resp = await req.ExecuteGet<TvShow>().ConfigureAwait(false);

            return resp;
        }

        /// <summary>
        /// Retrieves all information for a specific tv show in relation to the current user account
        /// </summary>
        /// <param name="tvShowId">The id of the tv show to get the account states for</param>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<AccountState> GetTvShowAccountStateAsync(int tvShowId)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest req = _client.Create("tv/{tvShowId}/{method}");
            req.AddUrlSegment("tvShowId", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("method", TvShowMethods.AccountStates.GetDescription());
            AddSessionId(req, SessionType.UserSession);

            RestResponse<AccountState> response = await req.ExecuteGet<AccountState>().ConfigureAwait(false);

            return await response.GetDataObject().ConfigureAwait(false);
        }

        public async Task<ResultContainer<AlternativeTitle>> GetTvShowAlternativeTitlesAsync(int id)
        {
            return await GetTvShowMethod<ResultContainer<AlternativeTitle>>(id, TvShowMethods.AlternativeTitles).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve a tv Show by id.
        /// </summary>
        /// <param name="id">TMDb id of the tv show to retrieve.</param>
        /// <param name="extraMethods">Enum flags indicating any additional data that should be fetched in the same request.</param>
        /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es </param>
        /// <param name="includeSeasonDetails">If true the client will retrieve extended season details such as it's underlying episodes</param>
        /// <returns>The requested Tv Show</returns>
        public async Task<TvShow> GetTvShowAsync(int id, TvShowMethods extraMethods = TvShowMethods.Undefined, string language = null, bool includeSeasonDetails = false)
        {
            JObject dynamicResult = await GetTvShowInternalAsync(id, extraMethods, language, includeSeasonDetails).ConfigureAwait(false);
            // Deserialize the base show information
            TvShow item = dynamicResult.ToObject<TvShow>(_client.Serializer);

            // No data to patch up so return
            if (item == null)
                return null;

            // Patch up data, so that the end user won't notice that we share objects between request-types.
            if (item.Translations != null)
                item.Translations.Id = id;

            if (item.AccountStates != null)
                item.AccountStates.Id = id;

            if (!includeSeasonDetails)
                return item;

            List<TvSeason> seasonsLight = item.Seasons;
            // Replace the bare bones version with the full versions we have retrieved so far
            item.Seasons =
                ((IEnumerable<KeyValuePair<string, JToken>>)dynamicResult)
                    .Where(x => x.Key.StartsWith("season/"))
                    .Select(x => x.Value.ToObject<TvSeason>(_client.Serializer))
                    .ToList();

            // Calculate and retrieve missing seasons
            int numberOfMissingSeasons = seasonsLight.Count - item.Seasons.Count;
            int maxSeasonNumber = seasonsLight.Max(x => x.SeasonNumber);
            while (numberOfMissingSeasons > 0)
            {
                int nextUnpopulatedSeason = item.Seasons.Max(x => x.SeasonNumber) + 1;
                JObject dynamicSeasonResult = await GetTvShowInternalAsync(id, TvShowMethods.Undefined, language, true, nextUnpopulatedSeason, maxSeasonNumber).ConfigureAwait(false);

                IEnumerable<TvSeason> additionalSeasons = ((IEnumerable<KeyValuePair<string, JToken>>)dynamicSeasonResult)
                    .Where(x => x.Key.StartsWith("season/"))
                    .Select(x => x.Value.ToObject<TvSeason>(_client.Serializer));
                foreach (var season in additionalSeasons)
                {
                    item.Seasons.Add(season);
                }

                numberOfMissingSeasons = seasonsLight.Count - item.Seasons.Count;
            }

            // Patch the season id's, at time of writing a _id is returned and does not match the normal season id's
            foreach (var seasonLight in seasonsLight)
                item.Seasons.First(s => s.SeasonNumber == seasonLight.SeasonNumber).Id = seasonLight.Id;

            return item;
        }

        private async Task<JObject> GetTvShowInternalAsync(int id, TvShowMethods extraMethods, string language, bool includeSeasonDetails = false, int seasonOffset = 0, int? maxSeasonNumber = null)
        {
            if (extraMethods.HasFlag(TvShowMethods.AccountStates))
                RequireSessionId(SessionType.UserSession);

            RestRequest req = _client.Create("tv/{id}");
            req.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));

            if (extraMethods.HasFlag(TvShowMethods.AccountStates))
                AddSessionId(req, SessionType.UserSession);

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            // Add all method flags
            var appendValues = Enum.GetValues(typeof(TvShowMethods))
                .OfType<TvShowMethods>()
                .Except(new[] { TvShowMethods.Undefined })
                .Where(s => extraMethods.HasFlag(s))
                .Select(s => s.GetDescription())
                .ToList();

            if (includeSeasonDetails)
            {
                int availableAppendCount = MaxNumberOfAppends - appendValues.Count;
                int limit;
                if (maxSeasonNumber == null)
                {
                    limit = availableAppendCount + seasonOffset;
                }
                else
                {
                    limit = (int)(maxSeasonNumber + 1);
                    // Check that the limit is does not exceed our available appends
                    limit = (limit - seasonOffset) > availableAppendCount ? (availableAppendCount + seasonOffset) : limit;
                }
                for (int i = seasonOffset; i < limit; i++)
                {
                    appendValues.Add($"season/{i}");
                }
            }

            req.AddParameter("append_to_response", string.Join(",", appendValues));

            RestResponse<JObject> response = await req.ExecuteGet<JObject>().ConfigureAwait(false);
            return await response.GetDataObject();
        }

        public async Task<ChangesContainer> GetTvShowChangesAsync(int id)
        {
            return await GetTvShowMethod<ChangesContainer>(id, TvShowMethods.Changes).ConfigureAwait(false);
        }

        public async Task<ResultContainer<ContentRating>> GetTvShowContentRatingsAsync(int id)
        {
            return await GetTvShowMethod<ResultContainer<ContentRating>>(id, TvShowMethods.ContentRatings).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns a credits object for the tv show associated with the provided TMDb id.
        /// </summary>
        /// <param name="id">The TMDb id of the target tv show.</param>
        /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es </param>
        public async Task<Credits> GetTvShowCreditsAsync(int id, string language = null)
        {
            return await GetTvShowMethod<Credits>(id, TvShowMethods.Credits, dateFormat: "yyyy-MM-dd", language: language).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns an object that contains all known exteral id's for the tv show related to the specified TMDB id.
        /// </summary>
        /// <param name="id">The TMDb id of the target tv show.</param>
        public async Task<ExternalIdsTvShow> GetTvShowExternalIdsAsync(int id)
        {
            return await GetTvShowMethod<ExternalIdsTvShow>(id, TvShowMethods.ExternalIds).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves all images all related to the specified tv show.
        /// </summary>
        /// <param name="id">The TMDb id of the target tv show.</param>
        /// <param name="language">
        /// If specified the api will attempt to return a localized result. ex: en,it,es.
        /// For images this means that the image might contain language specifc text
        /// </param>
        public async Task<ImagesWithId> GetTvShowImagesAsync(int id, string language = null)
        {
            return await GetTvShowMethod<ImagesWithId>(id, TvShowMethods.Images, language: language).ConfigureAwait(false);
        }

        public async Task<ResultContainer<Keyword>> GetTvShowKeywordsAsync(int id)
        {
            return await GetTvShowMethod<ResultContainer<Keyword>>(id, TvShowMethods.Keywords).ConfigureAwait(false);
        }

        private async Task<SearchContainer<SearchTv>> GetTvShowListAsync(int page, string language, string tvShowListType)
        {
            RestRequest req = _client.Create("tv/" + tvShowListType);

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page.ToString());

            RestResponse<SearchContainer<SearchTv>> response = await req.ExecuteGet<SearchContainer<SearchTv>>().ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Fetches a dynamic list of TV Shows
        /// </summary>
        /// <param name="list">Type of list to fetch</param>
        /// <param name="page">Page</param>
        /// <param name="timezone">Only relevant for list type AiringToday</param>
        /// <returns></returns>
        public async Task<SearchContainer<SearchTv>> GetTvShowListAsync(TvShowListType list, int page = 0, string timezone = null)
        {
            return await GetTvShowListAsync(list, DefaultLanguage, page, timezone).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetches a dynamic list of TV Shows
        /// </summary>
        /// <param name="list">Type of list to fetch</param>
        /// <param name="language">Language</param>
        /// <param name="page">Page</param>
        /// <param name="timezone">Only relevant for list type AiringToday</param>
        /// <returns></returns>
        public async Task<SearchContainer<SearchTv>> GetTvShowListAsync(TvShowListType list, string language, int page = 0, string timezone = null)
        {
            RestRequest req = _client.Create("tv/{method}");
            req.AddUrlSegment("method", list.GetDescription());

            if (page > 0)
                req.AddParameter("page", page.ToString());

            if (!string.IsNullOrEmpty(timezone))
                req.AddParameter("timezone", timezone);

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            RestResponse<SearchContainer<SearchTv>> resp = await req.ExecuteGet<SearchContainer<SearchTv>>().ConfigureAwait(false);

            return resp;
        }

        private async Task<T> GetTvShowMethod<T>(int id, TvShowMethods tvShowMethod, string dateFormat = null, string language = null, int page = 0) where T : new()
        {
            RestRequest req = _client.Create("tv/{id}/{method}");
            req.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("method", tvShowMethod.GetDescription());

            // TODO: Dateformat?
            //if (dateFormat != null)
            //    req.DateFormat = dateFormat;

            if (page > 0)
                req.AddParameter("page", page.ToString());

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            RestResponse<T> resp = await req.ExecuteGet<T>().ConfigureAwait(false);

            return resp;
        }

        /// <summary>
        /// Get the list of popular TV shows. This list refreshes every day.
        /// </summary>
        /// <returns>
        /// Returns the basic information about a tv show.
        /// For additional data use the main GetTvShowAsync method using the tv show id as parameter.
        /// </returns>
        public async Task<SearchContainer<SearchTv>> GetTvShowPopularAsync(int page = -1, string language = null)
        {
            return await GetTvShowListAsync(page, language, "popular").ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchTv>> GetTvShowSimilarAsync(int id, int page = 0)
        {
            return await GetTvShowSimilarAsync(id, DefaultLanguage, page).ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchTv>> GetTvShowSimilarAsync(int id, string language, int page)
        {
            return await GetTvShowMethod<SearchContainer<SearchTv>>(id, TvShowMethods.Similar, language: language, page: page).ConfigureAwait(false);
        }

        [Obsolete("Use GetTvShowPopularAsync")]
        public async Task<SearchContainer<SearchTv>> GetTvShowsPopularAsync(int page = -1, string language = null)
        {
            return await GetTvShowPopularAsync(page, language).ConfigureAwait(false);
        }

        [Obsolete("Use GetTvShowTopRatedAsync")]
        public async Task<SearchContainer<SearchTv>> GetTvShowsTopRatedAsync(int page = -1, string language = null)
        {
            return await GetTvShowTopRatedAsync(page, language).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the list of top rated TV shows. By default, this list will only include TV shows that have 2 or more votes. This list refreshes every day.
        /// </summary>
        /// <returns>
        /// Returns the basic information about a tv show.
        /// For additional data use the main GetTvShowAsync method using the tv show id as parameter
        /// </returns>
        public async Task<SearchContainer<SearchTv>> GetTvShowTopRatedAsync(int page = -1, string language = null)
        {
            return await GetTvShowListAsync(page, language, "top_rated").ConfigureAwait(false);
        }

        public async Task<TranslationsContainerTv> GetTvShowTranslationsAsync(int id)
        {
            return await GetTvShowMethod<TranslationsContainerTv>(id, TvShowMethods.Translations).ConfigureAwait(false);
        }

        public async Task<ResultContainer<Video>> GetTvShowVideosAsync(int id)
        {
            return await GetTvShowMethod<ResultContainer<Video>>(id, TvShowMethods.Videos).ConfigureAwait(false);
        }

        public async Task<bool> TvShowRemoveRatingAsync(int tvShowId)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest req = _client.Create("tv/{tvShowId}/rating");
            req.AddUrlSegment("tvShowId", tvShowId.ToString(CultureInfo.InvariantCulture));
            AddSessionId(req);

            RestResponse<PostReply> response = await req.ExecuteDelete<PostReply>().ConfigureAwait(false);

            // status code 13 = "The item/record was deleted successfully."
            PostReply item = await response.GetDataObject().ConfigureAwait(false);

            // TODO: Original code had a check for item=null
            return item.StatusCode == 13;
        }

        /// <summary>
        /// Change the rating of a specified tv show.
        /// </summary>
        /// <param name="tvShowId">The id of the tv show to rate</param>
        /// <param name="rating">The rating you wish to assign to the specified tv show. Value needs to be between 0.5 and 10 and must use increments of 0.5. Ex. using 7.1 will not work and return false.</param>
        /// <returns>True if the the tv show's rating was successfully updated, false if not</returns>
        /// <remarks>Requires a valid guest or user session</remarks>
        /// <exception cref="GuestSessionRequiredException">Thrown when the current client object doens't have a guest or user session assigned.</exception>
        public async Task<bool> TvShowSetRatingAsync(int tvShowId, double rating)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest req = _client.Create("tv/{tvShowId}/rating");
            req.AddUrlSegment("tvShowId", tvShowId.ToString(CultureInfo.InvariantCulture));
            AddSessionId(req);

            req.SetBody(new { value = rating });

            RestResponse<PostReply> response = await req.ExecutePost<PostReply>().ConfigureAwait(false);

            // status code 1 = "Success"
            // status code 12 = "The item/record was updated successfully" - Used when an item was previously rated by the user
            PostReply item = await response.GetDataObject().ConfigureAwait(false);

            // TODO: Original code had a check for item=null
            return item.StatusCode == 1 || item.StatusCode == 12;
        }
    }
}
