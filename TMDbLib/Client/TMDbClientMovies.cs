using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;
using TMDbLib.Utilities;
using Credits = TMDbLib.Objects.Movies.Credits;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Retrieves all information for a specific movie in relation to the current user account
        /// </summary>
        /// <param name="movieId">The id of the movie to get the account states for</param>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<AccountState> GetMovieAccountStateAsync(int movieId)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest req = _client.Create("movie/{movieId}/{method}");
            req.AddUrlSegment("movieId", movieId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("method", MovieMethods.AccountStates.GetDescription());
            AddSessionId(req, SessionType.UserSession);

            RestResponse<AccountState> response = await req.ExecuteGet<AccountState>().ConfigureAwait(false);

            return await response.GetDataObject().ConfigureAwait(false);
        }

        public async Task<AlternativeTitles> GetMovieAlternativeTitlesAsync(int movieId)
        {
            return await GetMovieAlternativeTitlesAsync(movieId, DefaultCountry).ConfigureAwait(false);
        }

        public async Task<AlternativeTitles> GetMovieAlternativeTitlesAsync(int movieId, string country)
        {
            return await GetMovieMethod<AlternativeTitles>(movieId, MovieMethods.AlternativeTitles, country: country).ConfigureAwait(false);
        }

        public async Task<Movie> GetMovieAsync(int movieId, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            return await GetMovieAsync(movieId, DefaultLanguage, extraMethods).ConfigureAwait(false);
        }

        public async Task<Movie> GetMovieAsync(string imdbId, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            return await GetMovieAsync(imdbId, DefaultLanguage, extraMethods).ConfigureAwait(false);
        }

        public async Task<Movie> GetMovieAsync(int movieId, string language, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            return await GetMovieAsync(movieId.ToString(CultureInfo.InvariantCulture), language, extraMethods).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves a movie by it's imdb Id
        /// </summary>
        /// <param name="imdbId">The Imdb id of the movie OR the TMDb id as string</param>
        /// <param name="language">Language to localize the results in.</param>
        /// <param name="extraMethods">A list of additional methods to execute for this req as enum flags</param>
        /// <returns>The reqed movie or null if it could not be found</returns>
        /// <remarks>Requires a valid user session when specifying the extra method 'AccountStates' flag</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned, see remarks.</exception>
        public async Task<Movie> GetMovieAsync(string imdbId, string language, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            if (extraMethods.HasFlag(MovieMethods.AccountStates))
                RequireSessionId(SessionType.UserSession);

            RestRequest req = _client.Create("movie/{movieId}");
            req.AddUrlSegment("movieId", imdbId);
            if (extraMethods.HasFlag(MovieMethods.AccountStates))
                AddSessionId(req, SessionType.UserSession);

            if (language != null)
                req.AddParameter("language", language);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(MovieMethods))
                                             .OfType<MovieMethods>()
                                             .Except(new[] { MovieMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            RestResponse<Movie> response = await req.ExecuteGet<Movie>().ConfigureAwait(false);

            // No data to patch up so return
            if (response == null) return null;

            Movie item = await response.GetDataObject().ConfigureAwait(false);

            // Patch up data, so that the end user won't notice that we share objects between req-types.
            if (item.Videos != null)
                item.Videos.Id = item.Id;

            if (item.AlternativeTitles != null)
                item.AlternativeTitles.Id = item.Id;

            if (item.Credits != null)
                item.Credits.Id = item.Id;

            if (item.Releases != null)
                item.Releases.Id = item.Id;

            if (item.Keywords != null)
                item.Keywords.Id = item.Id;

            if (item.Translations != null)
                item.Translations.Id = item.Id;

            if (item.AccountStates != null)
                item.AccountStates.Id = item.Id;

            // Overview is the only field that is HTML encoded from the source.
            item.Overview = WebUtility.HtmlDecode(item.Overview);

            return item;
        }

        public async Task<List<Change>> GetMovieChangesAsync(int movieId, DateTime? startDate = null, DateTime? endDate = null)
        {
            ChangesContainer changesContainer = await GetMovieMethod<ChangesContainer>(movieId, MovieMethods.Changes, startDate: startDate, endDate: endDate, dateFormat: "yyyy-MM-dd HH:mm:ss UTC").ConfigureAwait(false);
            return changesContainer.Changes;
        }

        public async Task<Credits> GetMovieCreditsAsync(int movieId)
        {
            return await GetMovieMethod<Credits>(movieId, MovieMethods.Credits).ConfigureAwait(false);
        }

        public async Task<ImagesWithId> GetMovieImagesAsync(int movieId)
        {
            return await GetMovieImagesAsync(movieId, DefaultLanguage).ConfigureAwait(false);
        }

        public async Task<ImagesWithId> GetMovieImagesAsync(int movieId, string language)
        {
            return await GetMovieMethod<ImagesWithId>(movieId, MovieMethods.Images, language: language).ConfigureAwait(false);
        }

        public async Task<KeywordsContainer> GetMovieKeywordsAsync(int movieId)
        {
            return await GetMovieMethod<KeywordsContainer>(movieId, MovieMethods.Keywords).ConfigureAwait(false);
        }

        public async Task<Movie> GetMovieLatestAsync()
        {
            RestRequest req = _client.Create("movie/latest");
            RestResponse<Movie> resp = await req.ExecuteGet<Movie>().ConfigureAwait(false);

            Movie item = await resp.GetDataObject().ConfigureAwait(false);

            // Overview is the only field that is HTML encoded from the source.
            if (item != null)
                item.Overview = WebUtility.HtmlDecode(item.Overview);

            return item;
        }

        public async Task<SearchContainerWithId<ListResult>> GetMovieListsAsync(int movieId, int page = 0)
        {
            return await GetMovieListsAsync(movieId, DefaultLanguage, page).ConfigureAwait(false);
        }

        public async Task<SearchContainerWithId<ListResult>> GetMovieListsAsync(int movieId, string language, int page = 0)
        {
            return await GetMovieMethod<SearchContainerWithId<ListResult>>(movieId, MovieMethods.Lists, page: page, language: language).ConfigureAwait(false);
        }

        private async Task<T> GetMovieMethod<T>(int movieId, MovieMethods movieMethod, string dateFormat = null,
            string country = null,
            string language = null, int page = 0, DateTime? startDate = null, DateTime? endDate = null) where T : new()
        {
            RestRequest req = _client.Create("movie/{movieId}/{method}");
            req.AddUrlSegment("movieId", movieId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("method", movieMethod.GetDescription());

            if (country != null)
                req.AddParameter("country", country);
            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (startDate.HasValue)
                req.AddParameter("start_date", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate != null)
                req.AddParameter("end_date", endDate.Value.ToString("yyyy-MM-dd"));

            RestResponse<T> response = await req.ExecuteGet<T>().ConfigureAwait(false);

            return response;
        }

        public async Task<SearchContainerWithDates<SearchMovie>> GetMovieNowPlayingListAsync(string language = null, int page = 0)
        {
            RestRequest req = _client.Create("movie/now_playing");

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (language != null)
                req.AddParameter("language", language);
                
            RestResponse<SearchContainerWithDates<SearchMovie>> resp = await req.ExecuteGet<SearchContainerWithDates<SearchMovie>>().ConfigureAwait(false);

            return resp;
        }

        public async Task<SearchContainer<SearchMovie>> GetMoviePopularListAsync(string language = null, int page = 0)
        {
            RestRequest req = _client.Create("movie/popular");

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (language != null)
                req.AddParameter("language", language);

            RestResponse<SearchContainer<SearchMovie>> resp = await req.ExecuteGet<SearchContainer<SearchMovie>>().ConfigureAwait(false);

            return resp;
        }

        public async Task<ResultContainer<ReleaseDatesContainer>> GetMovieReleaseDatesAsync(int movieId)
        {
            return await GetMovieMethod<ResultContainer<ReleaseDatesContainer>>(movieId, MovieMethods.ReleaseDates).ConfigureAwait(false);
        }

        public async Task<Releases> GetMovieReleasesAsync(int movieId)
        {
            return await GetMovieMethod<Releases>(movieId, MovieMethods.Releases, dateFormat: "yyyy-MM-dd").ConfigureAwait(false);
        }

        public async Task<SearchContainerWithId<ReviewBase>> GetMovieReviewsAsync(int movieId, int page = 0)
        {
            return await GetMovieReviewsAsync(movieId, DefaultLanguage, page).ConfigureAwait(false);
        }

        public async Task<SearchContainerWithId<ReviewBase>> GetMovieReviewsAsync(int movieId, string language, int page = 0)
        {
            return await GetMovieMethod<SearchContainerWithId<ReviewBase>>(movieId, MovieMethods.Reviews, page: page, language: language).ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchMovie>> GetMovieSimilarAsync(int movieId, int page = 0)
        {
            return await GetMovieSimilarAsync(movieId, DefaultLanguage, page).ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchMovie>> GetMovieSimilarAsync(int movieId, string language, int page = 0)
        {
            return await GetMovieMethod<SearchContainer<SearchMovie>>(movieId, MovieMethods.Similar, page: page, language: language, dateFormat: "yyyy-MM-dd").ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchMovie>> GetMovieTopRatedListAsync(string language = null, int page = 0)
        {
            RestRequest req = _client.Create("movie/top_rated");

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (language != null)
                req.AddParameter("language", language);

            RestResponse<SearchContainer<SearchMovie>> resp = await req.ExecuteGet<SearchContainer<SearchMovie>>().ConfigureAwait(false);

            return resp;
        }

        public async Task<TranslationsContainer> GetMovieTranslationsAsync(int movieId)
        {
            return await GetMovieMethod<TranslationsContainer>(movieId, MovieMethods.Translations).ConfigureAwait(false);
        }

        public async Task<SearchContainerWithDates<SearchMovie>> GetMovieUpcomingListAsync(string language = null, int page = 0)
        {
            RestRequest req = _client.Create("movie/upcoming");

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (language != null)
                req.AddParameter("language", language);

            RestResponse<SearchContainerWithDates<SearchMovie>> resp = await req.ExecuteGet<SearchContainerWithDates<SearchMovie>>().ConfigureAwait(false);

            return resp;
        }

        public async Task<ResultContainer<Video>> GetMovieVideosAsync(int movieId)
        {
            return await GetMovieMethod<ResultContainer<Video>>(movieId, MovieMethods.Videos).ConfigureAwait(false);
        }

        public async Task<bool> MovieRemoveRatingAsync(int movieId)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest req = _client.Create("movie/{movieId}/rating");
            req.AddUrlSegment("movieId", movieId.ToString(CultureInfo.InvariantCulture));
            AddSessionId(req);

            RestResponse<PostReply> response = await req.ExecuteDelete<PostReply>().ConfigureAwait(false);

            // status code 13 = "The item/record was deleted successfully."
            PostReply item = await response.GetDataObject().ConfigureAwait(false);

            // TODO: Previous code checked for item=null
            return item != null && item.StatusCode == 13;
        }

        /// <summary>
        /// Change the rating of a specified movie.
        /// </summary>
        /// <param name="movieId">The id of the movie to rate</param>
        /// <param name="rating">The rating you wish to assign to the specified movie. Value needs to be between 0.5 and 10 and must use increments of 0.5. Ex. using 7.1 will not work and return false.</param>
        /// <returns>True if the the movie's rating was successfully updated, false if not</returns>
        /// <remarks>Requires a valid guest or user session</remarks>
        /// <exception cref="GuestSessionRequiredException">Thrown when the current client object doens't have a guest or user session assigned.</exception>
        public async Task<bool> MovieSetRatingAsync(int movieId, double rating)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest req = _client.Create("movie/{movieId}/rating");
            req.AddUrlSegment("movieId", movieId.ToString(CultureInfo.InvariantCulture));
            AddSessionId(req);

            req.SetBody(new { value = rating });

            RestResponse<PostReply> response = await req.ExecutePost<PostReply>().ConfigureAwait(false);

            // status code 1 = "Success"
            // status code 12 = "The item/record was updated successfully" - Used when an item was previously rated by the user
            PostReply item = await response.GetDataObject().ConfigureAwait(false);

            // TODO: Previous code checked for item=null
            return item.StatusCode == 1 || item.StatusCode == 12;
        }
    }
}

