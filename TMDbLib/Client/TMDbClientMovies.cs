using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Reviews;
using TMDbLib.Utilities;
using Credits = TMDbLib.Objects.Movies.Credits;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Movie> GetMovie(int movieId, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            return await GetMovie(movieId, DefaultLanguage, extraMethods);
        }

        public async Task<Movie> GetMovie(string imdbId, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            return await GetMovie(imdbId, DefaultLanguage, extraMethods);
        }

        public async Task<Movie> GetMovie(int movieId, string language, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            return await GetMovie(movieId.ToString(CultureInfo.InvariantCulture), language, extraMethods);
        }

        /// <summary>
        /// Retrieves a movie by it's imdb Id
        /// </summary>
        /// <param name="imdbId">The Imdb id of the movie OR the TMDb id as string</param>
        /// <param name="language">Language to localize the results in.</param>
        /// <param name="extraMethods">A list of additional methods to execute for this request as enum flags</param>
        /// <returns>The requested movie or null if it could not be found</returns>
        /// <remarks>Requires a valid user session when specifying the extra method 'AccountStates' flag</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned, see remarks.</exception>
        public async Task<Movie> GetMovie(string imdbId, string language, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            if (extraMethods.HasFlag(MovieMethods.AccountStates))
                RequireSessionId(SessionType.UserSession);

            RestRequest request = new RestRequest("movie/{movieId}");
            request.AddUrlSegment("movieId", imdbId);
            if (extraMethods.HasFlag(MovieMethods.AccountStates))
                request.AddParameter("session_id", SessionId);

            if (language != null)
                request.AddParameter("language", language);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(MovieMethods))
                                             .OfType<MovieMethods>()
                                             .Except(new[] { MovieMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                request.AddParameter("append_to_response", appends);

            IRestResponse<Movie> response = await _client.ExecuteGetTaskAsync<Movie>(request).ConfigureAwait(false);

            // No data to patch up so return
            if (response.Data == null) return null;

            // Patch up data, so that the end user won't notice that we share objects between request-types.
            if (response.Data.Videos != null)
                response.Data.Videos.Id = response.Data.Id;

            if (response.Data.AlternativeTitles != null)
                response.Data.AlternativeTitles.Id = response.Data.Id;

            if (response.Data.Credits != null)
                response.Data.Credits.Id = response.Data.Id;

            if (response.Data.Releases != null)
                response.Data.Releases.Id = response.Data.Id;

            if (response.Data.Keywords != null)
                response.Data.Keywords.Id = response.Data.Id;

            if (response.Data.Translations != null)
                response.Data.Translations.Id = response.Data.Id;

            if (response.Data.AccountStates != null)
            {
                response.Data.AccountStates.Id = response.Data.Id;
                // Do some custom deserialization, since TMDb uses a property that changes type we can't use automatic deserialization
                CustomDeserialization.DeserializeAccountStatesRating(response.Data.AccountStates, response.Content);
            }

            return response.Data;
        }

        private async Task<T> GetMovieMethod<T>(int movieId, MovieMethods movieMethod, string dateFormat = null,
            string country = null,
            string language = null, int page = 0, DateTime? startDate = null, DateTime? endDate = null) where T : new()
        {
            RestRequest request = new RestRequest("movie/{movieId}/{method}");
            request.AddUrlSegment("movieId", movieId.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("method", movieMethod.GetDescription());

            if (dateFormat != null)
                request.DateFormat = dateFormat;

            if (country != null)
                request.AddParameter("country", country);
            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            if (page >= 1)
                request.AddParameter("page", page);
            if (startDate.HasValue)
                request.AddParameter("start_date", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate != null)
                request.AddParameter("end_date", endDate.Value.ToString("yyyy-MM-dd"));

            IRestResponse<T> response = await _client.ExecuteGetTaskAsync<T>(request).ConfigureAwait(false);

            return response.Data;
        }

        public async Task<AlternativeTitles> GetMovieAlternativeTitles(int movieId)
        {
            return await GetMovieAlternativeTitles(movieId, DefaultCountry);
        }

        public async Task<AlternativeTitles> GetMovieAlternativeTitles(int movieId, string country)
        {
            return await GetMovieMethod<AlternativeTitles>(movieId, MovieMethods.AlternativeTitles, country: country);
        }

        public async Task<Credits> GetMovieCredits(int movieId)
        {
            return await GetMovieMethod<Credits>(movieId, MovieMethods.Credits);
        }

        public async Task<ImagesWithId> GetMovieImages(int movieId)
        {
            return await GetMovieImages(movieId, DefaultLanguage);
        }

        public async Task<ImagesWithId> GetMovieImages(int movieId, string language)
        {
            return await GetMovieMethod<ImagesWithId>(movieId, MovieMethods.Images, language: language);
        }

        public async Task<KeywordsContainer> GetMovieKeywords(int movieId)
        {
            return await GetMovieMethod<KeywordsContainer>(movieId, MovieMethods.Keywords);
        }

        public async Task<Releases> GetMovieReleases(int movieId)
        {
            return await GetMovieMethod<Releases>(movieId, MovieMethods.Releases, dateFormat: "yyyy-MM-dd");
        }

        public async Task<ResultContainer<Video>> GetMovieVideos(int movieId)
        {
            return await GetMovieMethod<ResultContainer<Video>>(movieId, MovieMethods.Videos);
        }

        public async Task<TranslationsContainer> GetMovieTranslations(int movieId)
        {
            return await GetMovieMethod<TranslationsContainer>(movieId, MovieMethods.Translations);
        }

        public async Task<SearchContainer<MovieResult>> GetMovieSimilar(int movieId, int page = 0)
        {
            return await GetMovieSimilar(movieId, DefaultLanguage, page);
        }

        public async Task<SearchContainer<MovieResult>> GetMovieSimilar(int movieId, string language, int page = 0)
        {
            return await GetMovieMethod<SearchContainer<MovieResult>>(movieId, MovieMethods.Similar, page: page, language: language, dateFormat: "yyyy-MM-dd");
        }

        public async Task<SearchContainer<Review>> GetMovieReviews(int movieId, int page = 0)
        {
            return await GetMovieReviews(movieId, DefaultLanguage, page);
        }

        public async Task<SearchContainer<Review>> GetMovieReviews(int movieId, string language, int page = 0)
        {
            return await GetMovieMethod<SearchContainer<Review>>(movieId, MovieMethods.Reviews, page: page, language: language);
        }

        public async Task<SearchContainer<ListResult>> GetMovieLists(int movieId, int page = 0)
        {
            return await GetMovieLists(movieId, DefaultLanguage, page);
        }

        public async Task<SearchContainer<ListResult>> GetMovieLists(int movieId, string language, int page = 0)
        {
            return await GetMovieMethod<SearchContainer<ListResult>>(movieId, MovieMethods.Lists, page: page, language: language);
        }

        public async Task<List<Change>> GetMovieChanges(int movieId, DateTime? startDate = null, DateTime? endDate = null)
        {
            return (await GetMovieMethod<ChangesContainer>(movieId, MovieMethods.Changes, startDate: startDate, endDate: endDate, dateFormat: "yyyy-MM-dd HH:mm:ss UTC")).Changes;
        }

        /// <summary>
        /// Retrieves all information for a specific movie in relation to the current user account
        /// </summary>
        /// <param name="movieId">The id of the movie to get the account states for</param>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public async Task<AccountState> GetMovieAccountState(int movieId)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest request = new RestRequest("movie/{movieId}/{method}");
            request.AddUrlSegment("movieId", movieId.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("method", MovieMethods.AccountStates.GetDescription());
            request.AddParameter("session_id", SessionId);

            IRestResponse<AccountState> response = await _client.ExecuteGetTaskAsync<AccountState>(request).ConfigureAwait(false);

            // Do some custom deserialization, since TMDb uses a property that changes type we can't use automatic deserialization
            if (response.Data != null)
            {
                CustomDeserialization.DeserializeAccountStatesRating(response.Data, response.Content);
            }

            return response.Data;
        }

        /// <summary>
        /// Change the rating of a specified movie.
        /// </summary>
        /// <param name="movieId">The id of the movie to rate</param>
        /// <param name="rating">The rating you wish to assign to the specified movie. Value needs to be between 0.5 and 10 and must use increments of 0.5. Ex. using 7.1 will not work and return false.</param>
        /// <returns>True if the the movie's rating was successfully updated, false if not</returns>
        /// <remarks>Requires a valid guest or user session</remarks>
        /// <exception cref="GuestSessionRequiredException">Thrown when the current client object doens't have a guest or user session assigned.</exception>
        public async Task<bool> MovieSetRating(int movieId, double rating)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest request = new RestRequest("movie/{movieId}/rating") { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("movieId", movieId.ToString(CultureInfo.InvariantCulture));
            if (SessionType == SessionType.UserSession)
                request.AddParameter("session_id", SessionId, ParameterType.QueryString);
            else
                request.AddParameter("guest_session_id", SessionId, ParameterType.QueryString);

            request.AddBody(new { value = rating });

            IRestResponse<PostReply> response = await _client.ExecutePostTaskAsync<PostReply>(request).ConfigureAwait(false);

            // status code 1 = "Success"
            // status code 12 = "The item/record was updated successfully" - Used when an item was previously rated by the user
            return response.Data != null && (response.Data.StatusCode == 1 || response.Data.StatusCode == 12);
        }

        public async Task<Movie> GetMovieLatest()
        {
            RestRequest req = new RestRequest("movie/latest");
            IRestResponse<Movie> resp = await _client.ExecuteGetTaskAsync<Movie>(req).ConfigureAwait(false);

            return resp.Data;
        }

        public async Task<SearchContainer<MovieResult>> GetMovieList(MovieListType type, int page = 0)
        {
            return await GetMovieList(type, DefaultLanguage, page);
        }

        public async Task<SearchContainer<MovieResult>> GetMovieList(MovieListType type, string language, int page = 0)
        {
            RestRequest req;
            switch (type)
            {
                case MovieListType.NowPlaying:
                    req = new RestRequest("movie/now_playing");
                    break;
                case MovieListType.Popular:
                    req = new RestRequest("movie/popular");
                    break;
                case MovieListType.TopRated:
                    req = new RestRequest("movie/top_rated");
                    break;
                case MovieListType.Upcoming:
                    req = new RestRequest("movie/upcoming");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (language != null)
                req.AddParameter("language", language);

            req.DateFormat = "yyyy-MM-dd";

            IRestResponse<SearchContainer<MovieResult>> resp = await _client.ExecuteGetTaskAsync<SearchContainer<MovieResult>>(req).ConfigureAwait(false);

            return resp.Data;
        }
    }
}

