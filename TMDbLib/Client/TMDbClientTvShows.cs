using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLib.Utilities;

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
            RestRequest req = new RestRequest("tv/{id}");
            req.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(TvShowMethods))
                                             .OfType<TvShowMethods>()
                                             .Except(new[] { TvShowMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            IRestResponse<TvShow> response = await _client.ExecuteGetTaskAsync<TvShow>(req).ConfigureAwait(false);

            return response.Data;
        }

        /// <summary>
        /// Get the list of popular TV shows. This list refreshes every day.
        /// </summary>
        /// <returns>
        /// Returns the basic information about a tv show.
        /// For additional data use the main GetTvShow method using the tv show id as parameter.
        /// </returns>
        public async Task<SearchContainer<TvShowBase>> GetTvShowsPopular(int page = -1, string language = null)
        {
            return await GetTvShowList(page, language, "popular");
        }

        /// <summary>
        /// Get the list of top rated TV shows. By default, this list will only include TV shows that have 2 or more votes. This list refreshes every day.
        /// </summary>
        /// <returns>
        /// Returns the basic information about a tv show.
        /// For additional data use the main GetTvShow method using the tv show id as parameter
        /// </returns>
        public async Task<SearchContainer<TvShowBase>> GetTvShowsTopRated(int page = -1, string language = null)
        {
            return await GetTvShowList(page, language, "top_rated");
        }

        private async Task<SearchContainer<TvShowBase>> GetTvShowList(int page, string language, string tvShowListType)
        {
            RestRequest req = new RestRequest("tv/" + tvShowListType);

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page);

            IRestResponse<SearchContainer<TvShowBase>> response = await _client.ExecuteGetTaskAsync<SearchContainer<TvShowBase>>(req).ConfigureAwait(false);

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

        private async Task<T> GetTvShowMethod<T>(int id, TvShowMethods tvShowMethod, string dateFormat = null, string language = null) where T : new()
        {
            RestRequest req = new RestRequest("tv/{id}/{method}");
            req.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("method", tvShowMethod.GetDescription());

            if (dateFormat != null)
                req.DateFormat = dateFormat;

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            IRestResponse<T> resp = await _client.ExecuteGetTaskAsync<T>(req).ConfigureAwait(false);

            return resp.Data;
        }
    }
}
